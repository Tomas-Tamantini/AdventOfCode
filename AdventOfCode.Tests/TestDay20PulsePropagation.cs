namespace AdventOfCode.Tests
{
    public class TestDay20PulsePropagation
    {
        [Fact]
        public void TestFlipFlopModuleIsOffByDefault()
        {
            var module = new FlipFlopModule { Id = "test" };
            Assert.False(module.IsOn);
        }

        [Fact]
        public void TestFlipFlopIgnoresHighPulse()
        {
            var flipFlopOn = new FlipFlopModule { Id = "on", IsOn = true };
            var flipFlopOff = new FlipFlopModule { Id = "off", IsOn = false };
            var pulse = new Pulse(null, null, PulseIntensity.High);

            var resultingPulsesOn = flipFlopOn.EmitPulses(pulse);
            Assert.Empty(resultingPulsesOn);
            Assert.True(flipFlopOn.IsOn);


            var resultingPulsesOff = flipFlopOff.EmitPulses(pulse);
            Assert.Empty(resultingPulsesOff);
            Assert.False(flipFlopOff.IsOn);
        }

        [Fact]
        public void TestLowPulseTogglesFlipFlopState()
        {
            var flipFlopOn = new FlipFlopModule { Id = "on", IsOn = true };
            var flipFlopOff = new FlipFlopModule { Id = "off", IsOn = false };
            var pulse = new Pulse(null, null, PulseIntensity.Low);
            flipFlopOn.EmitPulses(pulse);
            Assert.False(flipFlopOn.IsOn);

            flipFlopOff.EmitPulses(pulse);
            Assert.True(flipFlopOff.IsOn);
        }

        [Fact]
        public void TestLowPulseIsSentToDestinationsInOrderByFlipFlop_SignalTurnsHighIfFlipFlopWasOff()
        {
            var destinationA = new PulseModule { Id = "a" };
            var destinationB = new PulseModule { Id = "b" };
            var flipFlopOn = new FlipFlopModule { Id = "on", IsOn = true };
            var flipFlopOff = new FlipFlopModule { Id = "off", IsOn = false };

            flipFlopOn.AddDestinations(destinationA, destinationB);
            flipFlopOff.AddDestinations(destinationB, destinationA);

            var pulse = new Pulse(null, null, PulseIntensity.Low);
            var resultingPulsesOn = flipFlopOn.EmitPulses(pulse);
            Assert.Equal(2, resultingPulsesOn.Count);
            Assert.Equal(new Pulse(flipFlopOn, destinationA, PulseIntensity.Low), resultingPulsesOn[0]);
            Assert.Equal(new Pulse(flipFlopOn, destinationB, PulseIntensity.Low), resultingPulsesOn[1]);

            var resultingPulsesOff = flipFlopOff.EmitPulses(pulse);
            Assert.Equal(2, resultingPulsesOff.Count);
            Assert.Equal(new Pulse(flipFlopOff, destinationB, PulseIntensity.High), resultingPulsesOff[0]);
            Assert.Equal(new Pulse(flipFlopOff, destinationA, PulseIntensity.High), resultingPulsesOff[1]);
        }

        [Fact]
        public void TestConjuctionModuleStartsOffWithAllLowPulses()
        {
            var originA = new PulseModule { Id = "a" };
            var originB = new PulseModule { Id = "b" };
            var destination = new PulseModule { Id = "c" };
            var conjuction = new ConjuctionModule { Id = "conj" };
            conjuction.AddOrigins(originA, originB);
            conjuction.AddDestinations(destination);
            Assert.Equal(2, conjuction.NumLowPulsesFromOrigins());
        }

        [Fact]
        public void TestConjuctionModuleReturnsHighPulseIfNotAllInputsAreHigh()
        {
            var originA = new PulseModule { Id = "a" };
            var originB = new PulseModule { Id = "b" };
            var destination = new PulseModule { Id = "c" };
            var conjuction = new ConjuctionModule { Id = "conj" };
            conjuction.AddOrigins(originA, originB);
            conjuction.AddDestinations(destination);
            var pulse = new Pulse(originA, conjuction, PulseIntensity.High);
            var resultingPulses = conjuction.EmitPulses(pulse);
            Assert.Single(resultingPulses);
            Assert.Equal(new Pulse(conjuction, destination, PulseIntensity.High), resultingPulses[0]);
        }

        [Fact]
        public void TestConjuctionModuleReturnsLowPulseIfAllInputsAreHigh()
        {
            var originA = new PulseModule { Id = "a" };
            var originB = new PulseModule { Id = "b" };
            var destination = new PulseModule { Id = "c" };
            var conjuction = new ConjuctionModule { Id = "conj" };
            conjuction.AddOrigins(originA, originB);
            conjuction.AddDestinations(destination);
            var pulseA = new Pulse(originA, conjuction, PulseIntensity.High);
            var pulseB = new Pulse(originB, conjuction, PulseIntensity.High);
            conjuction.EmitPulses(pulseA);
            var resultingPulses = conjuction.EmitPulses(pulseB);
            Assert.Single(resultingPulses);
            Assert.Equal(new Pulse(conjuction, destination, PulseIntensity.Low), resultingPulses[0]);
        }

        [Fact]
        public void TestBroadcastModuleSendsUnchangedPulseToAllDestinations()
        {
            var destinationA = new PulseModule { Id = "b" };
            var destinationB = new PulseModule { Id = "c" };
            var broadcast = new BroadcastModule { Id = "test" };
            broadcast.AddDestinations(destinationA, destinationB);
            var pulse = new Pulse(null, broadcast, PulseIntensity.Low);
            var resultingPulses = broadcast.EmitPulses(pulse);
            Assert.Equal(2, resultingPulses.Count);
            Assert.Equal(new Pulse(broadcast, destinationA, PulseIntensity.Low), resultingPulses[0]);
            Assert.Equal(new Pulse(broadcast, destinationB, PulseIntensity.Low), resultingPulses[1]);
        }

        private static PulseCircuit BasicCircuit()
        {
            BroadcastModule broadcaster = new() { Id = "broadcaster" };
            FlipFlopModule flipFlopA = new() { Id = "a" };
            FlipFlopModule flipFlopB = new() { Id = "b" };
            FlipFlopModule flipFlopC = new() { Id = "c" };
            ConjuctionModule conjuctionModule = new() { Id = "inv" };

            broadcaster.AddDestinations(flipFlopA, flipFlopB, flipFlopC);
            flipFlopA.AddDestinations(flipFlopB);
            flipFlopB.AddDestinations(flipFlopC);
            flipFlopC.AddDestinations(conjuctionModule);
            conjuctionModule.AddDestinations(flipFlopA);
            conjuctionModule.AddOrigins(flipFlopC);

            PulseCircuit circuit = new(broadcaster);
            return circuit;
        }

        private static PulseCircuit PeriodicCircuit()
        {
            BroadcastModule broadcaster = new() { Id = "broadcaster" };
            FlipFlopModule flipFlopA = new() { Id = "a" };
            FlipFlopModule flipFlopB = new() { Id = "b" };
            ConjuctionModule inv = new() { Id = "inv" };
            ConjuctionModule con = new() { Id = "con" };
            PulseModule output = new() { Id = "output" };

            broadcaster.AddDestinations(flipFlopA);
            flipFlopA.AddDestinations(inv, con);
            flipFlopB.AddDestinations(con);
            inv.AddDestinations(flipFlopB);
            con.AddDestinations(output);
            inv.AddOrigins(flipFlopA);
            con.AddOrigins(flipFlopA, flipFlopB);

            PulseCircuit circuit = new(broadcaster);
            return circuit;
        }

        [Fact]
        public void TestCircuitKeepsTrackOfHowManyLowAndHighPulsesWereSent()
        {
            PulseCircuit circuit = BasicCircuit();

            (int numLowPulses, int numHighPulses) = circuit.SendInitialPulse(PulseIntensity.Low);
            Assert.Equal(8, numLowPulses);
            Assert.Equal(4, numHighPulses);
        }



        [Fact]
        public void TestPulsePropagationKeepsTrackOfHighAndLowPulsesThroughMultipleCircuitRuns()
        {
            PulsePropagation propagation = new(BasicCircuit());

            (int numLowPulses, int numHighPulses) = propagation.RunCircuit(numTimes: 1000, PulseIntensity.Low);
            Assert.Equal(8000, numLowPulses);
            Assert.Equal(4000, numHighPulses);

            propagation = new(PeriodicCircuit());
            (numLowPulses, numHighPulses) = propagation.RunCircuit(numTimes: 1000, PulseIntensity.Low);
            Assert.Equal(4250, numLowPulses);
            Assert.Equal(2750, numHighPulses);
        }
    }
}