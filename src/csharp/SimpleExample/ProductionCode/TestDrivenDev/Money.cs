namespace ProductionCode.TestDrivenDev
{
    public abstract class Money
    {
        protected int Amount;

        public static Money Dollar(int amount)
        {
            return new Dollar(amount);
        }

        public static Money Franc(int amount)
        {
            return new Franc(amount);
        }

        public override bool Equals(object obj)
        {
            var money = (Money)obj;

            return Amount == money.Amount
                && GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public abstract Money Times(int multiplier);
    }

    public class Dollar : Money
    {
        public Dollar(int amount)
        {
            Amount = amount;
        }

        public override Money Times(int multiplier)
        {
            return new Dollar(Amount * multiplier);
        }
    }

    public class Franc : Money
    {
        public Franc(int amount)
        {
            Amount = amount;
        }

        public override Money Times(int multiplier)
        {
            return new Franc(Amount * multiplier);
        }
    }

}