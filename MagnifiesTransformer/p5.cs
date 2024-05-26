namespace MagnifiesTransformer;

class P5
{
    private const int NumObjects = 5;

    static void Main()
    {
        Console.WriteLine("-------------------------YOU ARE IN MAGNIFIER TRANSFORMER DRIVER!-------------------------");
        Console.WriteLine();
    }

    static void Execute(Magnifier[] magnifiers, TMagnifier[] tmagnifiers)
    {
        
    }


    static void AttemptYieldSize(Magnifier magnifier)
    {
        try
        {
            double returnSize = magnifier.YieldSize();
            Console.WriteLine($"New size yielded is : {returnSize}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void AttemptChangeMaxYield(Magnifier magnifier, uint newMaxYield)
    {
        Console.WriteLine("Changing maximum chances to yield");
        magnifier.ChangeMaxYield(newMaxYield);
    }

    static void AttemptReset(Magnifier magnifier)
    {
        Console.WriteLine("Resetting the object");
        magnifier.Reset();
    }

    static void AttemptActivate(Magnifier magnifier)
    {
        try
        {
            Console.WriteLine("Activating the object");
            magnifier.Activate();
            Console.WriteLine("Activate success!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            Console.WriteLine("Activate failed!");
        }
    }

    static void AttemptDeactivate(Magnifier magnifier)
    {
        try
        {
            Console.WriteLine("Deactivating the object");
            magnifier.Deactivate();
            Console.WriteLine("Deactivate success!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            Console.WriteLine("Deactivate failed!");
        }
    }

    static void GetStats(Magnifier magnifier)
    {
        Console.WriteLine($"Object's current size is : {magnifier.Size}");
        Console.WriteLine($"Object's scale factor is : {magnifier.ScaleFactor}");
        Console.WriteLine($"Object's minimum times to yield is : {magnifier.Limit}");
        Console.WriteLine($"Object's current chances to yield is : {magnifier.MaxYield}");
        Console.WriteLine($"Number of queries to yield size is : {magnifier.NumQueries}");
        Console.WriteLine($"Object in initial state : {magnifier.GetInitialState}");
        Console.WriteLine($"Object current state is : {magnifier.GetState}");
        Console.WriteLine($"Object's current yield direction is : {magnifier.GetDirection}");
    }
    
    
    static void AttemptTransform(TMagnifier tmagnifier, int guessValue)
    {
        try
        {
            double returnVal = tmagnifier.TMagnifierTransform(guessValue);
            Console.WriteLine(returnVal == 0 ? "Guess is correct!" : "Guess is not correct!");
            Console.WriteLine($"return value is {returnVal}");
        }
        catch (Exception ex)
        {   // for clarification, if active
            // a guess is only invalid if it hits the threshold -> cannot decide how to transform
            if (ex.Message != "Value collides with threshold")
            {
                Console.WriteLine(ex.Message);
            }
            else
            {
                // last case : guess a known target value -> object not active
                Console.WriteLine(ex.Message);
            }
        }
    }
    
    static void AttemptReset(TMagnifier tmagnifier)
    {
        Console.WriteLine("Resetting the object");
        tmagnifier.TMagnifierReset();
    }

    static void AttemptActivate(TMagnifier tmagnifier)
    {
        try
        {   
            Console.WriteLine("Activating the object");
            tmagnifier.TMagnifierActivate();
            Console.WriteLine("Activate success!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            Console.WriteLine("Activate failed!");
        }
    }

    static void AttemptDeactivate(TMagnifier tmagnifier)
    {
        try
        {   
            Console.WriteLine("Deactivating the object");
            tmagnifier.TMagnifierDeactivate();
            Console.WriteLine("Deactivate success!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            Console.WriteLine("Deactivate failed!");
        }
    }

    static void GetStats(TMagnifier tmagnifier)
    {
        Console.WriteLine($"Number of queries is : {tmagnifier.TMagnifierGetNumQueries}");
        Console.WriteLine($"Number of high guesses is : {tmagnifier.TMagnifierGetHighData}");
        Console.WriteLine($"Number of low guesses is : {tmagnifier.TMagnifierGetLowData}");
        Console.WriteLine($"Object in initial state : {tmagnifier.GetInitialState}");
        Console.WriteLine($"Target value is known : {tmagnifier.IsTargetKnown}");
        Console.WriteLine($"Object current state is : {tmagnifier.GetState}");
        Console.WriteLine($"Object's operation in last operation is : {tmagnifier.TMagnifierOperationType}");

        if (tmagnifier.GetTransformerPower() == typeof(AccelerateTransformer))
        {
            Console.WriteLine($"Accelerating factor of this object is {tmagnifier.GetAcceleratingFactor()}");
        }
        else if (tmagnifier.GetTransformerPower() == typeof(ViralTransformer))
        {
            Console.WriteLine($"Modulo factor of this object is {tmagnifier.GetModuloFactor()}");
        }

        Console.WriteLine();
        Console.WriteLine("ADDITIONAL INFORMATION");
        Console.WriteLine($"Operation used is Sum {tmagnifier.TMagnifierSum}");
        Console.WriteLine($"Operation used is Difference {tmagnifier.TMagnifierDifference}");
        Console.WriteLine($"Operation used is Product {tmagnifier.TMagnifierProduct}");
        Console.WriteLine($"Operation used is Modulo {tmagnifier.TMagnifierModulo}");
        Console.WriteLine();
    }
    


    private const int MinLimit = -10;
    private const int MaxLimit = 100;
    private const int TransformerId = 0;
    private const int AccelerateTransformerId = 2;
    private const int ViralTransformerId = 4;
    private const int AccelerateMinLimit = 1; // not makes sense to be 0
    private const int AccelerateMaxLimit = MaxLimit;
    private const double ModuloMinLimit = MinLimit;
    private const double ModuloMaxLimit = MaxLimit;
    private const int MinSize = 0; // not reasonable to have negative size
    private const int MaxSize = MaxLimit;
    private const int decimalPlaces = 2;
    private const int MinNumYield = 1;
    private const int MaxNumYield = MaxLimit;

    static Magnifier[] CreateMagnifiers(int num)
    {
        Random randNum = new Random();
        Magnifier[] magnifiers = new Magnifier[num];

        for (int i = 0; i < num; i++)
        {
            // initialize data will be used in all objects
            int size = randNum.Next(MinSize, MaxSize);
            double randDouble = randNum.NextDouble();
            double scaleFactor = MinLimit + (Math.Round(randDouble, decimalPlaces) * (MaxLimit - MinLimit));
            uint limit = Convert.ToUInt32(randNum.Next(MinNumYield, MaxNumYield));
            uint maxYield = Convert.ToUInt32(randNum.Next(Convert.ToInt32(limit), MaxLimit));
            
            if (i % 2 == 0) // strategy to select which object should be TMagnifier -> rest will be Magnifier
            {               // -> heterogeneous collection with various types of object
                int targetNum = randNum.Next(MinLimit, MaxLimit);
                switch (i) // strategy to integrate powers of different transformers into TMagnifier
                {          // -> multiple TMagnifiers with different transforming powers
                    case TransformerId:
                        int threshold = randNum.Next(MinLimit, MaxLimit);
                        magnifiers[i] = new TMagnifier(size, scaleFactor, limit, maxYield,
                            new Transformer(targetNum, threshold));
                        break;
                    
                    case AccelerateTransformerId:
                        int accelerator = randNum.Next(AccelerateMinLimit, AccelerateMaxLimit);
                        magnifiers[i] = new TMagnifier(size, scaleFactor, limit, maxYield,
                            new AccelerateTransformer(targetNum, accelerator));
                        break;
                    case ViralTransformerId:
                        randDouble = randNum.NextDouble();
                        double modulo = ModuloMinLimit +
                                        Math.Round(randDouble, decimalPlaces) * (ModuloMaxLimit - ModuloMinLimit);
                        try
                        {
                            magnifiers[i] = new TMagnifier(size, scaleFactor, limit, maxYield,
                                new ViralTransformer(targetNum, modulo));
                        }
                        catch (Exception)
                        {
                            i -= 1; // redo process
                        }

                        break;
                    default:
                        return [];
                }
            }
            else
            {
                magnifiers[i] = new Magnifier(size, scaleFactor, limit, maxYield);
            }
        }

        return magnifiers;
    }

    static TMagnifier[] CreateTMagnifiers(int num)
    {
        Random randNum = new Random();
        TMagnifier[] tmagnifiers = new TMagnifier[num];

        for (int i = 0; i < num; i++)
        {
            // initialize data will be used in all objects
            int size = randNum.Next(MinSize, MaxSize);
            double randDouble = randNum.NextDouble();
            double scaleFactor = MinLimit + (Math.Round(randDouble, decimalPlaces) * (MaxLimit - MinLimit));
            uint limit = Convert.ToUInt32(randNum.Next(MinNumYield, MaxNumYield));
            uint maxYield = Convert.ToUInt32(randNum.Next(Convert.ToInt32(limit), MaxLimit));
            
            int targetNum = randNum.Next(MinLimit, MaxLimit);
            switch (i) // strategy to integrate powers of different transformers into TMagnifier
            {
                // -> multiple TMagnifiers with different transforming powers
                case TransformerId:
                    int threshold = randNum.Next(MinLimit, MaxLimit);
                    tmagnifiers[i] = new TMagnifier(size, scaleFactor, limit, maxYield,
                        new Transformer(targetNum, threshold));
                    break;

                case AccelerateTransformerId:
                    int accelerator = randNum.Next(AccelerateMinLimit, AccelerateMaxLimit);
                    tmagnifiers[i] = new TMagnifier(size, scaleFactor, limit, maxYield,
                        new AccelerateTransformer(targetNum, accelerator));
                    break;
                case ViralTransformerId:
                    randDouble = randNum.NextDouble();
                    double modulo = ModuloMinLimit +
                                    Math.Round(randDouble, decimalPlaces) * (ModuloMaxLimit - ModuloMinLimit);
                    try
                    {
                        tmagnifiers[i] = new TMagnifier(size, scaleFactor, limit, maxYield,
                            new ViralTransformer(targetNum, modulo));
                    }
                    catch (Exception)
                    {
                        i -= 1; // redo process
                    }

                    break;
                default:
                    return [];
            }
        }

        return tmagnifiers;
    }
}