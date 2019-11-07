namespace VideoStore
{
    public class CustomerBuilder
    {
        private static int _customerCounter = 1;
        private string _name;

        public CustomerBuilder()
        {
            _name = "Customer" + _customerCounter.ToString("D3");
            _customerCounter++;
        }

        public CustomerBuilder WithName(
            string name)
        {
            _name = name;

            return this;
        }

        public Customer Create()
        {
            return new Customer(_name);
        }
    }
}