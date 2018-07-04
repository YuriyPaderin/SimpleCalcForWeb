namespace SimpleCalcForWeb
{
    public class ParsingItem
    {
        public double? Number { get; set; }
        public IMathOperation Operation { get; set; }
        public int Range { get; set; }
    }
}
