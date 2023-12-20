using System.IO.Pipes;

namespace AdventOfCode.Console.Models
{
    public enum PulseIntensity { Low, High }
    public record Pulse(PulseModule? Origin, PulseModule? Destination, PulseIntensity Intensity);
    public class PulseModule
    {
        public string Id { get; init; } = "";
        private List<PulseModule> Destinations { get; } = new();
        public virtual List<Pulse> EmitPulses(Pulse incomingPulse)
        {
            return Broadcast(incomingPulse.Intensity).ToList();
        }

        public void AddDestinations(params PulseModule[] destinations)
        {
            Destinations.AddRange(destinations);
        }

        protected IEnumerable<Pulse> Broadcast(PulseIntensity pulseIntensity)
        {
            foreach (var destination in Destinations)
            {
                yield return new Pulse(this, destination, pulseIntensity);
            }
        }
    }

    public class FlipFlopModule : PulseModule
    {
        public bool IsOn { get; set; } = false;
        public override List<Pulse> EmitPulses(Pulse incomingPulse)
        {
            if (incomingPulse.Intensity == PulseIntensity.High)
            {
                return new List<Pulse>();
            }
            IsOn = !IsOn;
            PulseIntensity newIntensity = IsOn ? PulseIntensity.High : PulseIntensity.Low;
            return Broadcast(newIntensity).ToList();
        }
    }

    public class ConjuctionModule : PulseModule
    {
        private readonly Dictionary<PulseModule, PulseIntensity> originIntensities = new();

        public void AddOrigins(params PulseModule[] origins)
        {
            foreach (var origin in origins)
            {
                originIntensities.Add(origin, PulseIntensity.Low);
            }
        }

        public int NumLowPulsesFromOrigins()
        {
            return originIntensities.Values.Count(i => i == PulseIntensity.Low);
        }

        public override List<Pulse> EmitPulses(Pulse incomingPulse)
        {
            originIntensities[incomingPulse.Origin!] = incomingPulse.Intensity;
            PulseIntensity outgoingIntensity = NumLowPulsesFromOrigins() == 0 ? PulseIntensity.Low : PulseIntensity.High;

            return Broadcast(outgoingIntensity).ToList();
        }
    }

    public class BroadcastModule : PulseModule { }

    public class PulseCircuit
    {
        private readonly BroadcastModule broadcastModule;
        public PulseCircuit(BroadcastModule broadcastModule)
        {
            this.broadcastModule = broadcastModule;
        }

        public (int, int) SendInitialPulse(PulseIntensity intensity)
        {
            int numLowPulses = 0;
            int numHighPulses = 0;

            Queue<Pulse> pulses = new();
            pulses.Enqueue(new Pulse(null, broadcastModule, intensity));

            while (pulses.Count > 0)
            {
                Pulse pulse = pulses.Dequeue();
                if (pulse.Destination is null) continue;
                if (pulse.Intensity == PulseIntensity.Low) numLowPulses++;
                else numHighPulses++;
                foreach (var newPulse in pulse.Destination.EmitPulses(pulse))
                {
                    pulses.Enqueue(newPulse);
                }
            }


            return (numLowPulses, numHighPulses);
        }
    }

    public class PulsePropagation
    {
        private readonly PulseCircuit circuit;

        public PulsePropagation(PulseCircuit circuit)
        {
            this.circuit = circuit;
        }

        public (int, int) RunCircuit(int numTimes, PulseIntensity initialPulseIntensity)
        {
            (int numLow, int numHigh) = (0, 0);
            for (int i = 0; i < numTimes; i++)
            {
                (int numLowIncrement, int numHighIncrement) = circuit.SendInitialPulse(initialPulseIntensity);
                numLow += numLowIncrement;
                numHigh += numHighIncrement;
            }
            return (numLow, numHigh);
        }
    }
}