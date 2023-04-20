using MyCalculationLibrary;

List<IBusinessType> businessTypes = new()
{
    new BusinessTypeOne(1, 11),
    new BusinessTypeTwo(2, 222),
    new BusinessTypeThree(3, 333),
    new BusinessTypeFour(4, 4444),
    new BusinessTypeMock(9, 99),
};
foreach (var businessType in businessTypes)
{
    var result = CalculateBusinessLogic.Calculate(businessType);
    Console.WriteLine(result);
}


namespace MyCalculationLibrary
{
    public interface IAnalogCalculator
    {
        decimal CalculateWeight(decimal left, decimal right);
    }

    public class AnalogCalculator : IAnalogCalculator, IDisposable
    {
        public decimal CalculateWeight(decimal left, decimal right)
        {
            Console.WriteLine($"CalculateWeight({left}, {right})");
            return left + right;
        }

        public void Dispose() => Console.WriteLine("Dispose()");
    }

    public class CalculateBusinessLogic
    {
        public static decimal Calculate(IBusinessType businessType)
        {
            using var calculator = new AnalogCalculator();
            return businessType.Calculate(calculator);
        }
    }

    public interface IBusinessType
    {
        decimal Calculate(IAnalogCalculator analogCalculator);
    }

    public abstract class BusinessTypeBase : IBusinessType
    {
        private const int Five = 5;
        protected const decimal Fifth = 0.5m;
        protected const decimal Seventh = 0.7m;
        protected const decimal Nineth = 0.9m;

        protected decimal Years { get; init; }
        protected decimal Amount { get; init; }

        protected BusinessTypeBase(int years, decimal amount)
        {
            Years = (decimal)((years > Five) ? Five : years) / 100;
            Amount = amount;
        }

        protected virtual decimal CalcLeft() => throw new NotSupportedException();
        protected virtual decimal CalcRight() => throw new NotSupportedException();

        public virtual decimal Calculate(IAnalogCalculator analogCalculator) => Amount;
    }

    public class BusinessTypeOne : BusinessTypeBase
    {
        public BusinessTypeOne(int years, decimal amount) : base(years, amount)
        {
        }
    }

    public class BusinessTypeTwo : BusinessTypeBase
    {
        public BusinessTypeTwo(int years, decimal amount) : base(years, amount) { }

        protected override decimal CalcLeft() => Amount * Nineth;
        protected override decimal CalcRight() => -Years * Amount * Nineth;

        public override decimal Calculate(IAnalogCalculator analogCalculator) =>
            analogCalculator.CalculateWeight(CalcLeft(), CalcRight());
    }

    public class BusinessTypeThree : BusinessTypeBase
    {
        public BusinessTypeThree(int years, decimal amount) : base(years, amount) { }
        protected override decimal CalcLeft() => Seventh * (Amount - Years);
        protected override decimal CalcRight() => Amount;

        public override decimal Calculate(IAnalogCalculator analogCalculator) =>
            analogCalculator.CalculateWeight(CalcLeft(), CalcRight());
    }
    public class BusinessTypeFour : BusinessTypeBase
    {
        public BusinessTypeFour(int years, decimal amount) : base(years, amount) { }
        protected override decimal CalcLeft() => Amount;

        protected override decimal CalcRight() => -Fifth * Amount;

        public override decimal Calculate(IAnalogCalculator analogCalculator) =>
            analogCalculator.CalculateWeight(CalcLeft(), CalcRight()) - Years * Amount * Fifth;
    }

    public class BusinessTypeMock : BusinessTypeBase
    {
        public BusinessTypeMock(int years, decimal amount) : base(years, amount) { }

        public override decimal Calculate(IAnalogCalculator analogCalculator) =>
            analogCalculator.CalculateWeight(Years, 0) + Amount;
    }
}