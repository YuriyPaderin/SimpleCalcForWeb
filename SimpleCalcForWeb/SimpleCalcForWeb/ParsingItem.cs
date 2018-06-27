namespace SimpleCalcForWeb
{
    public class ParsingItem
    {
        private double _number;
        private bool _isFilled;

        public bool IsFilledNumber { get; private set; }
        public double Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                IsFilledNumber = true;
            }
        }
        public IMathOperation Operation { get; set; }
        public int Range { get; set; }
        public bool IsFilled
        {
            get
            {
                //Если поля заполенены, возврощаем true, иначе false
                _isFilled = (IsFilledNumber && Operation != null);
                return _isFilled;
            }
        }
    }
}
