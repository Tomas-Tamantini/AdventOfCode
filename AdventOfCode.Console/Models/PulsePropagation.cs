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
        public int NumLowPulses { get; private set; } = 0;
        public int NumHighPulses { get; private set; } = 0;
        public List<PulseIntensity> PulseHistory { get; } = new();

        public PulseCircuit(BroadcastModule broadcastModule)
        {
            this.broadcastModule = broadcastModule;
        }

        public void SendInitialPulse(PulseIntensity intensity, string watchModuleId = "")
        {
            Reset();
            Queue<Pulse> pulses = new();
            pulses.Enqueue(new Pulse(null, broadcastModule, intensity));

            while (pulses.Count > 0)
            {
                Pulse pulse = pulses.Dequeue();
                if (pulse.Destination is null) continue;
                if (pulse.Intensity == PulseIntensity.Low) NumLowPulses++;
                else NumHighPulses++;
                if (pulse.Destination.Id == watchModuleId) PulseHistory.Add(pulse.Intensity);
                foreach (var newPulse in pulse.Destination.EmitPulses(pulse))
                {
                    pulses.Enqueue(newPulse);
                }
            }
        }

        private void Reset()
        {
            NumLowPulses = 0;
            NumHighPulses = 0;
            PulseHistory.Clear();
        }
    }

    public class PulsePropagation
    {
        private readonly PulseCircuit circuit;

        public PulsePropagation(PulseCircuit circuit)
        {
            this.circuit = circuit;
        }

        public (int, int) RunCircuitAndCountPulses(int numTimes, PulseIntensity initialPulseIntensity)
        {
            (int numLow, int numHigh) = (0, 0);
            for (int i = 0; i < numTimes; i++)
            {
                circuit.SendInitialPulse(initialPulseIntensity);
                numLow += circuit.NumLowPulses;
                numHigh += circuit.NumHighPulses;
            }
            return (numLow, numHigh);
        }

        public int RunCircuitUntilGivenPulse(PulseIntensity initialPulseIntensity, string moduleToMonitor, PulseIntensity intensityToMonitor)
        {
            int numIterations = 1;
            while (true)
            {
                circuit.SendInitialPulse(initialPulseIntensity, moduleToMonitor);
                if (circuit.PulseHistory.Contains(intensityToMonitor)) break;
                numIterations++;
            }
            return numIterations;
        }
    }
}