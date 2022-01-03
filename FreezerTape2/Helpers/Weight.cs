namespace FreezerTape2.Helpers
{
    public class Weight
    {
        public double Value { get; set; }
        public string Unit { get; set; }

        public Weight()
        {
            Value = 0;
            Unit = "g";
        }

        public Weight(double? value)
        {
            Value = value.GetValueOrDefault();
            Unit = "g";
        }

        public override string ToString()
        {
            return this.Value + " " + this.Unit;
        }
    }
}
