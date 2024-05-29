namespace MagnifiesTransformer;

class P5
{
    //-------------------------------------------------CONSTANTS INITIALIZATION ZONE------------------------------------
    private const int NumObjects = 5;
    private const int MinLimit = -10;
    private const int MaxLimit = 50;  // small max limit because some numbers gonna be large
    private const int TransformerId = 0;
    private const int AccelerateTransformerId = 1;
    private const int ViralTransformerId = 2;
    private const int LowestId = 0;
    private const int HighestId = 3;
    private const int AccelerateMinLimit = 1; // not makes sense to be 0
    private const int AccelerateMaxLimit = MaxLimit;
    private const double ModuloMinLimit = MinLimit;
    private const double ModuloMaxLimit = MaxLimit;
    private const int MinSize = 0; // not reasonable to have negative size
    private const int MaxSize = MaxLimit;
    private const int DecimalPlaces = 2;
    private const int MinNumYield = 1;
    private const int MaxNumYield = MaxLimit;
    private const int MinScale = 0;
    private const int MaxScale = MaxLimit;
    private const int YieldTimes = 2; //  just enough -> focus on other functionalities


    //-------------------------------------------------MAIN ENTRY OF THE PROGRAM----------------------------------------
    static void Main()
    {
        Console.WriteLine("-------------------------YOU ARE IN MAGNIFIER TRANSFORMER DRIVER!-------------------------");
        Console.WriteLine();

        // heterogeneous collection of TMagnifiers as Magnifiers to test Magnifier
        // tmagnifiers array test both Transformer and Magnifier cross-product functionalities
        Magnifier[] magnifiers = CreateMagnifiers(NumObjects);
        TMagnifier[] tmagnifiers = CreateTMagnifiers(NumObjects);
        Execute(magnifiers);
        Execute(tmagnifiers);
    }

    //-----------------------------------------KEY METHODS TO RUN ALL TESTING LOGIC-------------------------------------
    static void Execute(Magnifier[] magnifiers)
    {
        Console.WriteLine("\n--------------------------TESTING MAGNIFIERS--------------------------------------------");

        int identifier = 0;
        Random randNum = new Random();

        foreach (Magnifier magnifier in magnifiers)
        {
            identifier += 1;
            Console.WriteLine(
                $"\n--------------------------TESTING STATISTICS MAGNIFIER {identifier}--------------------------");
            Console.WriteLine();
            Console.WriteLine("EXPECTED STATISTICS");
            Console.WriteLine();
            Console.WriteLine("Number of queries is : 0");
            Console.WriteLine("Object in initial state : True");
            Console.WriteLine("Object current state is : Active");
            Console.WriteLine("Object's current yield direction is : Unknown");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("REAL STATISTICS");
            Console.WriteLine();

            GetStats(magnifier); // some additional stats are not predictable

            Console.WriteLine(
                $"\n--------------------------TESTING DEACTIVATE MAGNIFIER {identifier}--------------------------");
            AttemptDeactivate(magnifier);

            Console.WriteLine();
            Console.WriteLine("EXPECTED STATES");
            Console.WriteLine("Object current state is : Inactive");
            Console.WriteLine("Object is active : False");
            Console.WriteLine("Object is inactive : True");
            Console.WriteLine("Object is being shut down : False");

            Console.WriteLine();
            Console.WriteLine("REAL STATISTICS");
            Console.WriteLine($"Object current state is : {magnifier.GetState}");
            Console.WriteLine($"Object is active : {magnifier.IsActive}");
            Console.WriteLine($"Object is inactive : {magnifier.IsInactive}");
            Console.WriteLine($"Object is being shut down : {magnifier.IsShutDown}");

            Console.WriteLine(
                $"\n--------------------------TESTING ACTIVATE MAGNIFIER {identifier}--------------------------");
            AttemptActivate(magnifier);

            Console.WriteLine();
            Console.WriteLine("EXPECTED STATES");
            Console.WriteLine("Object current state is : Active");
            Console.WriteLine("Object is active : True");
            Console.WriteLine("Object is inactive : False");
            Console.WriteLine("Object is being shut down : False");

            Console.WriteLine();
            Console.WriteLine("REAL STATES");
            Console.WriteLine($"Object current state is : {magnifier.GetState}");
            Console.WriteLine($"Object is active : {magnifier.IsActive}");
            Console.WriteLine($"Object is inactive : {magnifier.IsInactive}");
            Console.WriteLine($"Object is being shut down : {magnifier.IsShutDown}");

            Console.WriteLine(
                $"\n--------------------------TESTING YIELD SIZE MAGNIFIER {identifier}--------------------------");

            Console.WriteLine("\nREPORTING STATISTICS (YIELD 2 TIMES) \n");
            for (int i = 0; i < YieldTimes; i++)
            {
                // each object guess 2 times
                AttemptYieldSize(magnifier);
            }

            // reporting statistics after 2 yields for each object
            GetStats(magnifier);
        }

        Console.WriteLine("\n--------------------------TESTING A MIX OF METHODS AND STATES--------------------------");
        Console.WriteLine("\nCHANGE MAXIMUM NUMBER OF TIMES TO YIELD FOR A FEW OBJECTS\n");

        for (int i = 0; i < magnifiers.Length; i += 2)
        {
            identifier = i + 1;
            Console.WriteLine($"\n--------------------------MAGNIFIER {identifier}--------------------------------");
            // Alter states of a few objects
            uint currentYield = magnifiers[i].MaxYield;
            Console.WriteLine("\nBEFORE CHANGING MAX NUMBER OF TIMES TO YIELD\n");
            GetStats(magnifiers[i]);

            Console.WriteLine("\nAFTER CHANGING MAX NUMBER OF TIMES TO YIELD\n");
            AttemptChangeMaxYield(magnifiers[i], Convert.ToUInt32(randNum.Next(MinNumYield, MaxNumYield)));

            Console.WriteLine();
            GetStats(magnifiers[i]); // show new reflected maxYield with additional updated data
            Console.WriteLine();

            Console.WriteLine("\nEXPECTED RESULT\n");
            Console.WriteLine();
            Console.WriteLine($"Magnifier {identifier} current number of chances to yield : {currentYield}");
            Console.WriteLine();

            Console.WriteLine("\nREAL RESULT\n");
            Console.WriteLine();
            Console.WriteLine($"Magnifier {identifier} current number of chances to yield : {magnifiers[i].MaxYield}");
            Console.WriteLine();
        }

        Console.WriteLine("\nDEACTIVATE AN OBJECT\n");
        for (int i = 0; i < magnifiers.Length; i += 2)
        {
            identifier = i + 1;

            if (!magnifiers[i].IsInactive)
            {
                if (magnifiers[i].IsActive)
                {
                    Console.WriteLine(
                        $"\n--------------------------MAGNIFIER {identifier}--------------------------------");
                    AttemptDeactivate(magnifiers[i]);
                }
                // attempt to deactivate one of them
                else if (magnifiers[i].IsShutDown)
                {
                    // reset then deactivate it
                    AttemptReset(magnifiers[i]);
                    AttemptDeactivate(magnifiers[i]);
                }

                Console.WriteLine();
                Console.WriteLine("\nEXPECTED RESULT\n");
                Console.WriteLine();
                Console.WriteLine("Object current state is : Inactive");

                Console.WriteLine("\nREAL RESULT\n");
                Console.WriteLine();
                Console.WriteLine($"Object current state is : {magnifiers[i].GetState}");
                break;
            }

            Console.WriteLine("Object is already inactive");
            Console.WriteLine();
        }

        Console.WriteLine("\nEXHAUST YIELD CHANCES OF AN OBJECT\n");
        for (int i = 0; i < magnifiers.Length; i += 2)
        {
            identifier = i + 1;

            if (magnifiers[i].IsActive)
            {
                Console.WriteLine(
                    $"\n--------------------------MAGNIFIER {identifier}--------------------------------");
                while (!magnifiers[i].IsShutDown)
                {
                    AttemptYieldSize(magnifiers[i]);
                }

                Console.WriteLine();
                Console.WriteLine("\nEXPECTED RESULT\n");
                Console.WriteLine("Object current state is : ShutDown");
                Console.WriteLine("Object is being shut down : True");
                Console.WriteLine();

                Console.WriteLine("\nREAL RESULT\n");
                Console.WriteLine();
                Console.WriteLine($"Object current state is : {magnifiers[i].GetState}");
                Console.WriteLine($"Object is being shut down : {magnifiers[i].IsShutDown}");
                break; // aim for one of them only, if found -> exhaust it to shut it down
            }
        }

        Console.WriteLine();
        Console.WriteLine("TEST YIELD SIZE WITH ALL OBJECTS (WITH A MIX OF STATES)");
        identifier = 0;
        foreach (Magnifier magnifier in magnifiers)
        {
            identifier += 1;
            Console.WriteLine($"\n--------------------------MAGNIFIER {identifier}--------------------------\n");
            Console.WriteLine("REPORTING STATISTICS (YIELD SIZE ANOTHER 2 TIMES)\n");

            for (int i = 0; i < YieldTimes; i++)
            {
                AttemptYieldSize(magnifier);
            }

            // reporting statistics after 2 more yields for each object
            GetStats(magnifier);
        }
    }

    static void Execute(TMagnifier[] tmagnifiers)
    {
        Console.WriteLine(tmagnifiers[0]);
        Console.WriteLine("\n--------------------------TESTING TMAGNIFIERS-------------------------------------------");
        int identifier = 0;
        const int numGuess = 2;
        Random randNum = new Random();
        foreach (TMagnifier tmagnifier in tmagnifiers)
        {
            identifier += 1;
            Console.WriteLine(
                $"\n--------------------------TESTING STATISTICS TMAGNIFIER {identifier}--------------------------");
            Console.WriteLine();
            Console.WriteLine("EXPECTED STATISTICS");
            Console.WriteLine();
            Console.WriteLine("Number of queries is : 0");
            Console.WriteLine("Number of high guesses is : 0");
            Console.WriteLine("Number of low guesses is : 0");
            Console.WriteLine("Object in initial state : True");
            Console.WriteLine("Target value is known : False");
            Console.WriteLine("Object current state is : Active");
            Console.WriteLine("Object's operation currently is : Unknown");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("REAL STATISTICS");
            Console.WriteLine();

            GetStats(tmagnifier);

            Console.WriteLine(
                $"\n--------------------------TESTING DEACTIVATE TMAGNIFIER {identifier}--------------------------");

            AttemptDeactivate(tmagnifier);

            Console.WriteLine();
            Console.WriteLine("EXPECTED STATES");
            Console.WriteLine("Object current state is : Inactive");
            Console.WriteLine("Object is active : False");
            Console.WriteLine("Object is inactive : True");
            Console.WriteLine("Object is being shut down : False");

            Console.WriteLine();
            Console.WriteLine("REAL STATISTICS");
            Console.WriteLine($"Object current state is : {tmagnifier.GetState}");
            Console.WriteLine($"Object is active : {tmagnifier.IsActive}");
            Console.WriteLine($"Object is inactive : {tmagnifier.IsInactive}");
            Console.WriteLine($"Object is being shut down : {tmagnifier.IsShutDown}");

            Console.WriteLine(
                $"\n--------------------------TESTING ACTIVATE TMAGNIFIER {identifier}--------------------------");

            AttemptActivate(tmagnifier);

            Console.WriteLine();
            Console.WriteLine("EXPECTED STATES");
            Console.WriteLine("Object current state is : Active");
            Console.WriteLine("Object is active : True");
            Console.WriteLine("Object is inactive : False");
            Console.WriteLine("Object is being shut down : False");

            Console.WriteLine();
            Console.WriteLine("REAL STATISTICS");
            Console.WriteLine($"Object current state is : {tmagnifier.GetState}");
            Console.WriteLine($"Object is active : {tmagnifier.IsActive}");
            Console.WriteLine($"Object is inactive : {tmagnifier.IsInactive}");
            Console.WriteLine($"Object is being shut down : {tmagnifier.IsShutDown}");

            Console.WriteLine(
                $"\n--------------------------TESTING TRANSFORM TMAGNIFIER {identifier}--------------------------");

            Console.WriteLine("\nREPORTING STATISTICS (GUESS 2 TIMES) \n");
            for (int i = 0; i < numGuess; i++)
            {
                // each object guess 2 times
                int randGuess = randNum.Next(MinLimit, MaxLimit);
                AttemptTransform(tmagnifier, randGuess);
                Console.WriteLine("Guess is: " + randGuess);
                Console.WriteLine($"Operation performed is : {tmagnifier.TMagnifierOperationType}");
                Console.WriteLine();
            }

            // reporting statistics after 3 guesses for each object
            GetStats(tmagnifier);

            Console.WriteLine(
                $"\n--------------------------TESTING YIELD SIZE TMAGNIFIER {identifier}--------------------------");
            Console.WriteLine("\nREPORTING STATISTICS (YIELD 2 TIMES) \n");
            for (int i = 0; i < YieldTimes; i++)
            {
                // each object guess 2 times -> prove that Tmagnifier can do both transformer + magnifier abilities
                AttemptYieldSize(tmagnifier); // numQueries is what will change
            }

            // reporting statistics after 2 yields for each object
            GetStats(tmagnifier);
        }

        Console.WriteLine("\n--------------------------TESTING A MIX OF METHODS AND STATES--------------------------");
        Console.WriteLine("\nRESET A FEW OBJECTS\n");

        for (int i = 0; i < tmagnifiers.Length; i += 2)
        {
            identifier = i + 1;
            Console.WriteLine($"\n--------------------------TMAGNIFIER {identifier}--------------------------------");
            // Alter states of a few objects
            Console.WriteLine("\nEXPECTED RESULT\n");
            Console.WriteLine();

            Console.WriteLine($"Object {identifier} is in initial state: False");
            Console.WriteLine();

            Console.WriteLine("\nREAL RESULT\n");
            Console.WriteLine();

            Console.WriteLine($"Object {identifier} is in initial state: {tmagnifiers[i].GetInitialState}");
            Console.WriteLine();

            AttemptReset(tmagnifiers[i]);

            Console.WriteLine();
            Console.WriteLine("EXPECTED RESULT");
            Console.WriteLine("Number of queries is : 0");
            Console.WriteLine("Number of high guesses is : 0");
            Console.WriteLine("Number of low guesses is : 0");
            Console.WriteLine("Object in initial state : True ");
            Console.WriteLine("Target value is known : False");
            Console.WriteLine("Object current state is : Active");
            Console.WriteLine("Object's operation in the last guess is : Unknown ");

            if (tmagnifiers[i].GetType() == typeof(AccelerateTransformer))
            {
                Console.WriteLine("Accelerating factor of this object is : randomized value at creation");
            }
            else if (tmagnifiers[i].GetType() == typeof(ViralTransformer))
            {
                Console.WriteLine("Modulo factor of this object is : randomized value at creation");
            }

            Console.WriteLine();


            Console.WriteLine("REAL RESULT");
            GetStats(tmagnifiers[i]);
        }

        Console.WriteLine("\nDEACTIVATE AN OBJECT\n");
        for (int i = 0; i < tmagnifiers.Length; i += 2)
        {
            identifier = i + 1;

            if (!tmagnifiers[i].IsInactive)
            {
                if (tmagnifiers[i].IsActive)
                {
                    Console.WriteLine(
                        $"\n--------------------------TMAGNIFIER {identifier}--------------------------------");
                    // attempt to deactivate one of them
                    AttemptDeactivate(tmagnifiers[i]);
                }

                else if (tmagnifiers[i].IsActive)
                {
                    // reset then deactivate it
                    AttemptReset(tmagnifiers[i]);
                    AttemptDeactivate(tmagnifiers[i]);
                }

                Console.WriteLine();
                Console.WriteLine("EXPECTED RESULT");
                Console.WriteLine("Object current state is : Inactive");

                Console.WriteLine("REAL RESULT");
                Console.WriteLine($"Object current state is : {tmagnifiers[i].GetState}");
                break;
            }

            Console.WriteLine("Object is already inactive");
        }

        Console.WriteLine();
        Console.WriteLine("TEST TRANSFORM WITH ALL OBJECTS (WITH A MIX OF STATES)");
        identifier = 0;
        foreach (TMagnifier tmagnifier in tmagnifiers)
        {
            identifier += 1;
            Console.WriteLine($"\n--------------------------TMAGNIFIER {identifier}--------------------------\n");
            Console.WriteLine("REPORTING STATISTICS (GUESS ANOTHER 2 TIMES) \n");

            for (int i = 0; i < numGuess; i++)
            {
                int randGuess = randNum.Next(MinLimit, MaxLimit);
                // each can guess 3 more times
                AttemptTransform(tmagnifier, randGuess);
                Console.WriteLine("Guess is: " + randGuess);
                Console.WriteLine($"Operation performed is : {tmagnifier.TMagnifierOperationType}");
                Console.WriteLine();
            }

            // reporting stats
            GetStats(tmagnifier);
        }
    }

    //------------------------------------------TESTING METHODS FOR MAGNIFIERS------------------------------------------
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
        Console.WriteLine($"Object's initial size is : {magnifier.Size}");
        Console.WriteLine($"Object's scale factor is : {magnifier.ScaleFactor}");
        Console.WriteLine($"Object's minimum required chances to yield is : {magnifier.Limit}");
        Console.WriteLine($"Object initial chances to yield is : {magnifier.GetInitialYield}");
        Console.WriteLine($"Object's current chances to yield is : {magnifier.MaxYield}");
        Console.WriteLine($"Number of queries to yield size is : {magnifier.NumQueries}");
        Console.WriteLine($"Object in initial state : {magnifier.GetInitialState}");
        Console.WriteLine($"Object current state is : {magnifier.GetState}");
        Console.WriteLine($"Object's current yield direction is : {magnifier.GetDirection}");
    }

    static Magnifier[] CreateMagnifiers(int num)
    {
        Random randNum = new Random();
        Magnifier[] magnifiers = new Magnifier[num];

        for (int i = 0; i < num; i++)
        {
            // initialize data will be used in all objects
            uint size = Convert.ToUInt32(randNum.Next(MinSize, MaxSize));
            double randDouble = randNum.NextDouble();
            double scaleFactor = MinScale + (Math.Round(randDouble, DecimalPlaces) * (MaxScale - MinScale));
            uint limit = Convert.ToUInt32(randNum.Next(MinNumYield, MaxNumYield));
            uint maxYield = Convert.ToUInt32(randNum.Next(Convert.ToInt32(limit), MaxLimit));


            if (i % 2 == 0) // strategy to select which object should be TMagnifier -> rest will be Magnifier
            {
                // -> heterogeneous collection with various types of object (both TMagnifiers and Magnifiers)
                int targetNum = randNum.Next(MinLimit, MaxLimit);
                int randID = randNum.Next(LowestId, HighestId);
                switch (randID) // strategy to integrate powers of different transformers into TMagnifier
                {
                    // -> multiple TMagnifiers with different transforming powers
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
                                        Math.Round(randDouble, DecimalPlaces) * (ModuloMaxLimit - ModuloMinLimit);
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

    //------------------------------------------TESTING METHODS FOR TMAGNIFIERS-----------------------------------------
    static void AttemptTransform(TMagnifier tmagnifier, int guessValue)
    {
        try
        {
            double returnVal = tmagnifier.TMagnifierTransform(guessValue);
            Console.WriteLine(returnVal == 0 ? "Guess is correct!" : "Guess is not correct!");
            Console.WriteLine($"return value is {returnVal}");
        }
        catch (Exception ex)
        {
            // for clarification, if active
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
        Console.WriteLine($"Number of queries is : {tmagnifier.NumQueries}");
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
        Console.WriteLine($"Operation used is Sum : {tmagnifier.TMagnifierSum}");
        Console.WriteLine($"Operation used is Difference : {tmagnifier.TMagnifierDifference}");
        Console.WriteLine($"Operation used is Product : {tmagnifier.TMagnifierProduct}");
        Console.WriteLine($"Operation used is Modulo : {tmagnifier.TMagnifierModulo}");
        Console.WriteLine();
    }

    static TMagnifier[] CreateTMagnifiers(int num)
    {
        Random randNum = new Random();
        TMagnifier[] tmagnifiers = new TMagnifier[num];

        for (int i = 0; i < num; i++)
        {
            // initialize data will be used in all objects
            uint size = Convert.ToUInt32(randNum.Next(MinSize, MaxSize));
            double randDouble = randNum.NextDouble();
            double scaleFactor = MinScale + (Math.Round(randDouble, DecimalPlaces) * (MaxScale - MinScale));
            uint limit = Convert.ToUInt32(randNum.Next(MinNumYield, MaxNumYield));
            uint maxYield = Convert.ToUInt32(randNum.Next(Convert.ToInt32(limit), MaxLimit));

            int targetNum = randNum.Next(MinLimit, MaxLimit);
            int randID = randNum.Next(LowestId, HighestId);
            switch (randID) // strategy to integrate powers of different transformers into TMagnifier
            {
                // -> multiple TMagnifiers with different transforming powers
                case TransformerId:
                    int threshold = randNum.Next(MinLimit, MaxLimit);
                    try
                    {
                        tmagnifiers[i] = new TMagnifier(size, scaleFactor, limit, maxYield,
                            new Transformer(targetNum, threshold));
                    }
                    catch (Exception)
                    {
                        threshold = randNum.Next(MinLimit, MaxLimit);
                        tmagnifiers[i] = new TMagnifier(size, scaleFactor, limit, maxYield,
                            new Transformer(targetNum, threshold));
                    }

                    break;


                case AccelerateTransformerId:
                    int accelerator = randNum.Next(AccelerateMinLimit, AccelerateMaxLimit);
                    tmagnifiers[i] = new TMagnifier(size, scaleFactor, limit, maxYield,
                        new AccelerateTransformer(targetNum, accelerator));
                    break;
                case ViralTransformerId:
                    randDouble = randNum.NextDouble();
                    double modulo = ModuloMinLimit +
                                    Math.Round(randDouble, DecimalPlaces) * (ModuloMaxLimit - ModuloMinLimit);
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