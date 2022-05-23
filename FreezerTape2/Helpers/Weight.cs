namespace FreezerTape2.Helpers
{
    /// <summary>
    /// Class to handle weights.
    /// </summary>
    public class Weight
    {
        public double Value { get; set; }
        public string Unit { get; set; }

        /// <summary>
        /// Creatse a new instace with default <see cref="Unit"/> "g".
        /// </summary>
        public Weight()
        {
            Value = 0;
            Unit = "g";
        }

        /// <summary>
        /// Creatse a new instace with default <see cref="Unit"/> "g".
        /// </summary>
        /// <param name="value">The wanted value.</param>
        public Weight(double? value)
        {
            Value = value.GetValueOrDefault();
            Unit = "g";
        }

        /// <summary>
        /// Returns a string with the value and unit combined.
        /// </summary>

        public override string ToString()
        {
            return this.Value + " " + this.Unit;
        }
    }
}
