namespace LW1
{
    public class Triangle
    {
        public Triangle(double sideA, double sideB, double sideC)
        {
            _sideA = sideA;
            _sideB = sideB;
            _sideC = sideC;
            ValidateTriangle(_sideA, _sideB, _sideC);
            ValidateTriangle(_sideB, _sideA, _sideC);
            ValidateTriangle(_sideC, _sideA, _sideB);
        }


        public string GetShape()
        {
            if (_shape == null)
            {
                if (IsSimple())
                    _shape = "обычный";
                if (IsIsosceles())
                    _shape = "равнобедренный";
                if (IsEquilateral())
                    _shape = "равносторонний";
            }
            return _shape;
        }

        private bool IsSimple()
        {
            return ((_sideA != _sideB) & (_sideB != _sideC));
        }

        private bool IsEquilateral()
        {
            return ((_sideA == _sideB) & (_sideB == _sideC));
        }

        private bool IsIsosceles()
        {
            return ((_sideA == _sideB) | (_sideB == _sideC) | (_sideA == _sideC));
        }

        private void ValidateTriangle(double sideToCheck, double sideA, double sideB)
        {
            if (sideToCheck > sideA + sideB)
                _shape = "не треугольник";
        }
        private double _sideA;
        private double _sideB;
        private double _sideC;
        private string _shape;
    }
}
