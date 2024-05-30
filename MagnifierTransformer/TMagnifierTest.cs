using MagnifiesTransformer;

namespace MagnifierTransformer;

public class TMagnifierTest
{
    [TestFixture]
    public class TMagnifierUnitTest
    {
        [Test]
        public void TestSuccessfulCreateTMagnifier()
        {
            // arbitrary values to help instantiate 3 tmagnifiers
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size, scaleFactor, limit, maxYield, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new ViralTransformer(targetNum3, modulo));
            
            Assert.IsTrue(tmagnifier1.IsActive);
            Assert.IsTrue(tmagnifier2.IsActive);
            Assert.IsTrue(tmagnifier3.IsActive);
            
            // verify correct data initializations for both Magnifier and Transformer (3 objects)
            Assert.IsTrue(tmagnifier1.GetInitialState);
            Assert.That(tmagnifier1.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier1.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier1.NumQueries, Is.EqualTo(0));
            Assert.IsTrue(tmagnifier1.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier1.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier1.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier1.MaxYield, Is.EqualTo(20));
            Assert.IsTrue(tmagnifier1.IsUnknown);
            
            Assert.IsTrue(tmagnifier2.GetInitialState);
            Assert.That(tmagnifier2.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier2.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier2.NumQueries, Is.EqualTo(0));
            Assert.IsTrue(tmagnifier2.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier2.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier2.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier2.MaxYield, Is.EqualTo(20));
            Assert.IsTrue(tmagnifier2.IsUnknown);
            
            Assert.IsTrue(tmagnifier3.GetInitialState);
            Assert.That(tmagnifier3.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier3.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier3.NumQueries, Is.EqualTo(0));
            Assert.IsTrue(tmagnifier3.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier3.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier3.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier3.MaxYield, Is.EqualTo(20));
            Assert.IsTrue(tmagnifier3.IsUnknown);
        }

        [Test]
        public void TestFailCreateTmagnifier()
        {
            // arbitrary values to help instantiate 3 tmagnifiers
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            const int threshold = 10;
            const int accelerator = -4;
            const double modulo = 0.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            Assert.Throws<Exception>(() =>
            {   // threshold collides with target value
                TMagnifier unused =
                    new TMagnifier(size, scaleFactor, limit, maxYield, new Transformer(targetNum1, threshold));
            });
            Assert.Throws<Exception>(() =>
            {   // accelerator is negative
                TMagnifier unused =
                    new TMagnifier(size, scaleFactor, limit, maxYield, new AccelerateTransformer(targetNum2, accelerator));
            });
            Assert.Throws<Exception>(() =>
            {   // modulo is 0
                TMagnifier unused =
                    new TMagnifier(size, scaleFactor, limit, maxYield, new ViralTransformer(targetNum3, modulo));
            });
        }

        [Test]
        public void TestSuccessfulTransform()
        {
            // arbitrary values to help instantiate 3 tmagnifiers
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size, scaleFactor, limit, maxYield, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new ViralTransformer(targetNum3, modulo));
            
            
            // guess 2 times for each Tmagnifier
            double returnVal1 = tmagnifier1.TMagnifierTransform(5);
            Assert.That(returnVal1, Is.EqualTo(15));
            Assert.IsFalse(tmagnifier1.GetInitialState);
            Assert.IsFalse(tmagnifier1.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier1.TMagnifierGetLowData, Is.EqualTo(1));
            Assert.That(tmagnifier1.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier1.NumQueries, Is.EqualTo(1));
            Assert.IsTrue(tmagnifier1.TMagnifierSum);

            returnVal1 = tmagnifier1.TMagnifierTransform(10);
            Assert.That(returnVal1, Is.EqualTo(0));
            Assert.IsFalse(tmagnifier1.GetInitialState);
            Assert.IsTrue(tmagnifier1.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier1.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier1.TMagnifierGetLowData, Is.EqualTo(1));
            Assert.That(tmagnifier1.NumQueries, Is.EqualTo(2));
            Assert.IsTrue(tmagnifier1.TMagnifierUnknownOperation);
            
            
            double returnVal2 = tmagnifier2.TMagnifierTransform(6);
            Assert.That(returnVal2, Is.EqualTo(24));
            Assert.IsFalse(tmagnifier2.GetInitialState);
            Assert.IsFalse(tmagnifier2.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier2.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier2.TMagnifierGetLowData, Is.EqualTo(1));
            Assert.That(tmagnifier2.NumQueries, Is.EqualTo(1));
            Assert.IsTrue(tmagnifier2.TMagnifierSum);

            returnVal2 = tmagnifier2.TMagnifierTransform(20);
            Assert.That(returnVal2, Is.EqualTo(0));
            Assert.IsFalse(tmagnifier2.GetInitialState);
            Assert.IsTrue(tmagnifier2.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier2.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier2.TMagnifierGetLowData, Is.EqualTo(1));
            Assert.That(tmagnifier2.NumQueries, Is.EqualTo(2));
            Assert.IsTrue(tmagnifier2.TMagnifierUnknownOperation);
            
            double returnVal3 = tmagnifier3.TMagnifierTransform(16);
            Assert.That(returnVal3, Is.EqualTo(0.0)); // overlapping value when guess is correct
            Assert.IsFalse(tmagnifier3.GetInitialState);  // but target is guaranteed still unknown
            Assert.IsFalse(tmagnifier3.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier3.TMagnifierGetLowData, Is.EqualTo(1));
            Assert.That(tmagnifier3.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier3.NumQueries, Is.EqualTo(1));
            Assert.IsTrue(tmagnifier3.TMagnifierModulo);

            returnVal3 = tmagnifier3.TMagnifierTransform(30);
            Assert.That(returnVal3, Is.EqualTo(0));
            Assert.IsFalse(tmagnifier3.GetInitialState);
            Assert.IsTrue(tmagnifier3.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier3.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier3.TMagnifierGetLowData, Is.EqualTo(1));
            Assert.That(tmagnifier3.NumQueries, Is.EqualTo(2));
            Assert.IsTrue(tmagnifier3.TMagnifierUnknownOperation);
        }

        [Test]
        public void TestFailTransform()
        {
            // arbitrary values to help instantiate 3 tmagnifiers
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size, scaleFactor, limit, maxYield, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new ViralTransformer(targetNum3, modulo));
            
            
            // guess 2 times for each Tmagnifier
            double returnVal1 = tmagnifier1.TMagnifierTransform(10);
            Assert.That(returnVal1, Is.EqualTo(0));
            Assert.IsFalse(tmagnifier1.GetInitialState);
            Assert.IsTrue(tmagnifier1.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier1.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier1.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier1.NumQueries, Is.EqualTo(1));
            Assert.IsTrue(tmagnifier1.TMagnifierUnknownOperation);

            // target already known
            Assert.Throws<Exception>(() =>
            {
                double unused = tmagnifier1.TMagnifierTransform(23);
            });
            
            

            double returnVal2 = tmagnifier2.TMagnifierTransform(20);
            Assert.That(returnVal2, Is.EqualTo(0));
            Assert.IsFalse(tmagnifier2.GetInitialState);
            Assert.IsTrue(tmagnifier2.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier2.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier2.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier2.NumQueries, Is.EqualTo(1));
            Assert.IsTrue(tmagnifier2.TMagnifierUnknownOperation);
            
            // target already known
            Assert.Throws<Exception>(() =>
            {
                double unused = tmagnifier2.TMagnifierTransform(23);
            });

            

            double returnVal3 = tmagnifier3.TMagnifierTransform(30);
            Assert.That(returnVal3, Is.EqualTo(0));
            Assert.IsFalse(tmagnifier3.GetInitialState);
            Assert.IsTrue(tmagnifier3.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier3.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier3.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier3.NumQueries, Is.EqualTo(1));
            Assert.IsTrue(tmagnifier3.TMagnifierUnknownOperation);
            
            // target already known
            Assert.Throws<Exception>(() =>
            {
                double unused = tmagnifier3.TMagnifierTransform(23);
            });

        }

        [Test]
        public void TestSuccessfulYieldSize()
        {
             // arbitrary values to help instantiate 3 tmagnifiers
            const uint size1 = 10;
            const uint size2 = 20;
            const uint size3 = 30;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield1 = 20;
            const uint maxYield2 = 40;
            const uint maxYield3 = 25;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size1, scaleFactor, limit, maxYield1, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size2, scaleFactor, limit, maxYield2,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size3, scaleFactor, limit, maxYield3,
                new ViralTransformer(targetNum3, modulo));

            // test Magnifier power after performing Transformer power to demonstrate
            // tmagnifier possesses both powers
            double newSize1 = tmagnifier1.YieldSize();
            
            Assert.IsFalse(tmagnifier1.GetInitialState);
            Assert.IsTrue(tmagnifier1.IsActive);
            Assert.IsTrue(tmagnifier1.IsUp);
            Assert.That(tmagnifier1.NumQueries, Is.EqualTo(1));
            Assert.That(newSize1, Is.EqualTo(25));
            Assert.That(tmagnifier1.MaxYield, Is.EqualTo(19));
            Assert.That(tmagnifier1.GetInitialYield, Is.EqualTo(20));
            
            double newSize2 = tmagnifier2.YieldSize();
            Assert.IsFalse(tmagnifier2.GetInitialState);
            Assert.IsTrue(tmagnifier2.IsActive);
            Assert.IsTrue(tmagnifier2.IsUp);
            Assert.That(tmagnifier2.NumQueries, Is.EqualTo(1));
            Assert.That(newSize2, Is.EqualTo(50));
            Assert.That(tmagnifier2.MaxYield, Is.EqualTo(39));
            Assert.That(tmagnifier2.GetInitialYield, Is.EqualTo(40));

            
            double newSize3 = tmagnifier3.YieldSize();
            Assert.IsFalse(tmagnifier3.GetInitialState);
            Assert.IsTrue(tmagnifier3.IsActive);
            Assert.IsTrue(tmagnifier3.IsUp);
            Assert.That(tmagnifier3.NumQueries, Is.EqualTo(1));
            Assert.That(newSize3, Is.EqualTo(75));
            Assert.That(tmagnifier3.MaxYield, Is.EqualTo(24));
            Assert.That(tmagnifier3.GetInitialYield, Is.EqualTo(25));
        }

        [Test]
        public void TestFailYieldSize()
        {
            const uint size1 = 10;
            const uint size2 = 20;
            const uint size3 = 30;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield1 = 2;
            const uint maxYield2 = 3;
            const uint maxYield3 = 4;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size1, scaleFactor, limit, maxYield1, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size2, scaleFactor, limit, maxYield2,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size3, scaleFactor, limit, maxYield3,
                new ViralTransformer(targetNum3, modulo));
            
            
            double newSize1 = tmagnifier1.YieldSize();
            
            Assert.IsFalse(tmagnifier1.GetInitialState);
            Assert.IsTrue(tmagnifier1.IsActive);
            Assert.IsTrue(tmagnifier1.IsDown);
            Assert.That(tmagnifier1.NumQueries, Is.EqualTo(1));
            Assert.That(newSize1, Is.EqualTo(4.0));
            Assert.That(tmagnifier1.MaxYield, Is.EqualTo(1));
            Assert.That(tmagnifier1.GetInitialYield, Is.EqualTo(2));
            
            double newSize2 = tmagnifier2.YieldSize();
            Assert.IsFalse(tmagnifier2.GetInitialState);
            Assert.IsTrue(tmagnifier2.IsActive);
            Assert.IsTrue(tmagnifier2.IsDown);
            Assert.That(tmagnifier2.NumQueries, Is.EqualTo(1));
            Assert.That(newSize2, Is.EqualTo(8.0));
            Assert.That(tmagnifier2.MaxYield, Is.EqualTo(2));
            Assert.That(tmagnifier2.GetInitialYield, Is.EqualTo(3));

            
            double newSize3 = tmagnifier3.YieldSize();
            Assert.IsFalse(tmagnifier3.GetInitialState);
            Assert.IsTrue(tmagnifier3.IsActive);
            Assert.IsTrue(tmagnifier3.IsDown);
            Assert.That(tmagnifier3.NumQueries, Is.EqualTo(1));
            Assert.That(newSize3, Is.EqualTo(12));
            Assert.That(tmagnifier3.MaxYield, Is.EqualTo(3));
            Assert.That(tmagnifier3.GetInitialYield, Is.EqualTo(4));
            
            Assert.Throws<Exception>(() =>
            {
                // variable is just for checking exception thrown purposes
                double unused = tmagnifier1.YieldSize(); // out of chances to yield
                tmagnifier1.YieldSize();
            });
            
            Assert.Throws<Exception>(() =>
            {
                // variable is just for checking exception thrown purposes
                double unused = tmagnifier2.YieldSize(); 
                tmagnifier2.YieldSize(); // out of chances to yield
                tmagnifier2.YieldSize();
            });
            
            Assert.Throws<Exception>(() =>
            {
                // variable is just for checking exception thrown purposes
                double unused = tmagnifier3.YieldSize(); 
                tmagnifier3.YieldSize(); // out of chances to yield
                tmagnifier3.YieldSize(); 
                tmagnifier3.YieldSize(); 
            });
        }

        [Test]
        public void TestSuccessfulReset() 
        {
            // arbitrary values to help instantiate 3 tmagnifiers
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size, scaleFactor, limit, maxYield, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new ViralTransformer(targetNum3, modulo));
            
            Assert.IsTrue(tmagnifier1.IsActive);
            Assert.IsTrue(tmagnifier2.IsActive);
            Assert.IsTrue(tmagnifier3.IsActive);
            
            // verify correct data initializations for both Magnifier and Transformer (3 objects)
            Assert.IsTrue(tmagnifier1.GetInitialState);
            Assert.That(tmagnifier1.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier1.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier1.NumQueries, Is.EqualTo(0));
            Assert.IsTrue(tmagnifier1.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier1.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier1.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier1.MaxYield, Is.EqualTo(20));
            Assert.IsTrue(tmagnifier1.IsUnknown);
            
            Assert.IsTrue(tmagnifier2.GetInitialState);
            Assert.That(tmagnifier2.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier2.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier2.NumQueries, Is.EqualTo(0));
            Assert.IsTrue(tmagnifier2.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier2.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier2.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier2.MaxYield, Is.EqualTo(20));
            Assert.IsTrue(tmagnifier2.IsUnknown);
            
            Assert.IsTrue(tmagnifier3.GetInitialState);
            Assert.That(tmagnifier3.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier3.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier3.NumQueries, Is.EqualTo(0));
            Assert.IsTrue(tmagnifier3.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier3.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier3.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier3.MaxYield, Is.EqualTo(20));
            Assert.IsTrue(tmagnifier3.IsUnknown);


            for (int i = 0; i < 4; i++)
            {   // aim to set objects away from initial state -> purpose is to reset
                tmagnifier1.TMagnifierTransform(i);              
                tmagnifier2.TMagnifierTransform(i);
                tmagnifier3.TMagnifierTransform(i);

                // purpose same as above -> waste return values but they will be unused
                tmagnifier1.YieldSize();
                tmagnifier2.YieldSize();
                tmagnifier3.YieldSize();

            }
            
            Assert.IsFalse(tmagnifier1.GetInitialState);
            Assert.That(tmagnifier1.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier1.TMagnifierGetLowData, Is.EqualTo(4));
            Assert.That(tmagnifier1.NumQueries, Is.EqualTo(8));
            Assert.IsFalse(tmagnifier1.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier1.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier1.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier1.MaxYield, Is.EqualTo(16));
            Assert.IsFalse(tmagnifier1.IsUnknown);
            
            Assert.IsFalse(tmagnifier2.GetInitialState);
            Assert.That(tmagnifier2.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier2.TMagnifierGetLowData, Is.EqualTo(4));
            Assert.That(tmagnifier2.NumQueries, Is.EqualTo(8));
            Assert.IsFalse(tmagnifier2.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier2.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier2.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier2.MaxYield, Is.EqualTo(16));
            Assert.IsFalse(tmagnifier2.IsUnknown);
            
            Assert.IsFalse(tmagnifier3.GetInitialState);
            Assert.That(tmagnifier3.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier3.TMagnifierGetLowData, Is.EqualTo(4));
            Assert.That(tmagnifier3.NumQueries, Is.EqualTo(8));
            Assert.IsFalse(tmagnifier3.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier3.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier3.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier3.MaxYield, Is.EqualTo(16));
            Assert.IsFalse(tmagnifier3.IsUnknown);


            tmagnifier1.TMagnifierReset();
            tmagnifier2.TMagnifierReset();
            tmagnifier3.TMagnifierReset();
            
            
            Assert.IsTrue(tmagnifier1.IsActive);
            Assert.IsTrue(tmagnifier2.IsActive);
            Assert.IsTrue(tmagnifier3.IsActive);
            
            // verify correct data initializations for both Magnifier and Transformer (3 objects)
            Assert.IsTrue(tmagnifier1.GetInitialState);
            Assert.That(tmagnifier1.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier1.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier1.NumQueries, Is.EqualTo(0));
            Assert.IsTrue(tmagnifier1.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier1.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier1.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier1.MaxYield, Is.EqualTo(20));
            Assert.IsTrue(tmagnifier1.IsUnknown);
            
            Assert.IsTrue(tmagnifier2.GetInitialState);
            Assert.That(tmagnifier2.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier2.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier2.NumQueries, Is.EqualTo(0));
            Assert.IsTrue(tmagnifier2.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier2.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier2.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier2.MaxYield, Is.EqualTo(20));
            Assert.IsTrue(tmagnifier2.IsUnknown);
            
            Assert.IsTrue(tmagnifier3.GetInitialState);
            Assert.That(tmagnifier3.TMagnifierGetHighData, Is.EqualTo(0));
            Assert.That(tmagnifier3.TMagnifierGetLowData, Is.EqualTo(0));
            Assert.That(tmagnifier3.NumQueries, Is.EqualTo(0));
            Assert.IsTrue(tmagnifier3.TMagnifierUnknownOperation);
            Assert.IsFalse(tmagnifier3.TMagnifierIsTargetKnown);
            Assert.That(tmagnifier3.GetInitialYield, Is.EqualTo(20));
            Assert.That(tmagnifier3.MaxYield, Is.EqualTo(20));
            Assert.IsTrue(tmagnifier3.IsUnknown);
        }

        [Test]
        public void TestSuccessfulActivate()
        {
            // arbitrary values to help instantiate 3 tmagnifiers
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size, scaleFactor, limit, maxYield, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new ViralTransformer(targetNum3, modulo));

            // deactivate to test activate
            tmagnifier1.TMagnifierDeactivate();
            tmagnifier2.TMagnifierDeactivate();
            tmagnifier3.TMagnifierDeactivate();

            // exceptions -> objects are not active
            Assert.Throws<Exception>(() => tmagnifier1.TMagnifierTransform(5));
            Assert.Throws<Exception>(() => tmagnifier2.TMagnifierTransform(5));
            Assert.Throws<Exception>(() => tmagnifier3.TMagnifierTransform(5));
            
            tmagnifier1.TMagnifierActivate();
            tmagnifier2.TMagnifierActivate();
            tmagnifier3.TMagnifierActivate();
            
            Assert.IsTrue(tmagnifier1.IsActive);
            Assert.IsTrue(tmagnifier2.IsActive );
            Assert.IsTrue(tmagnifier3.IsActive);
        }

        [Test]
        public void TestFailActivate()
        {
            // arbitrary values to help instantiate 3 tmagnifiers
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 1;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size, scaleFactor, limit, maxYield, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new ViralTransformer(targetNum3, modulo));

            // waste return values but will not be used -> no need variables
            // now all three are in shut down mode -> cannot activate
            tmagnifier1.YieldSize();
            tmagnifier2.YieldSize();
            tmagnifier3.YieldSize();
            
            Assert.Throws<Exception>(() => tmagnifier1.TMagnifierActivate());
            Assert.Throws<Exception>(() => tmagnifier2.TMagnifierActivate());
            Assert.Throws<Exception>(() => tmagnifier3.TMagnifierActivate());
        }

        [Test]
        public void TestSuccessfulDeactivate()
        {
            // arbitrary values to help instantiate 3 tmagnifiers
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size, scaleFactor, limit, maxYield, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new ViralTransformer(targetNum3, modulo));

            // same portion in TestSuccessfulActivate
            // objects need to be deactivated to test activation
            // -> that part also verify deactivation works -> this just give more details about states
            tmagnifier1.TMagnifierDeactivate();
            tmagnifier2.TMagnifierDeactivate();
            tmagnifier3.TMagnifierDeactivate();

            Assert.Throws<Exception>(() => tmagnifier1.TMagnifierTransform(5));
            Assert.Throws<Exception>(() => tmagnifier2.TMagnifierTransform(5));
            Assert.Throws<Exception>(() => tmagnifier3.TMagnifierTransform(5));

            Assert.IsTrue(tmagnifier1.IsInactive);
            Assert.IsTrue(tmagnifier2.IsInactive);
            Assert.IsTrue(tmagnifier3.IsInactive);
        }

        [Test]
        public void TestFailDeactivate()
        {
            // arbitrary values to help instantiate 3 tmagnifiers
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 1;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size, scaleFactor, limit, maxYield, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new ViralTransformer(targetNum3, modulo));

            // waste return values but will not be used -> no need variables
            // now all three are in shut down mode -> cannot deactivate
            tmagnifier1.YieldSize();
            tmagnifier2.YieldSize();
            tmagnifier3.YieldSize();
            
            Assert.Throws<Exception>(() => tmagnifier1.TMagnifierDeactivate());
            Assert.Throws<Exception>(() => tmagnifier2.TMagnifierDeactivate());
            Assert.Throws<Exception>(() => tmagnifier3.TMagnifierDeactivate());
        }

        [Test]
        public void TestSuccessfulGetAcceleratingFactor()
        {
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            const double accelerator = 5.0;
            const int targetNum = 30;
            
            TMagnifier tmagnifier = new TMagnifier(size, scaleFactor, limit, maxYield,
                new AccelerateTransformer(targetNum, accelerator));
            double acceleratingFactor = tmagnifier.GetAcceleratingFactor();

            // get acceleratingFactor and also verify if object is AccelerateTransformer
            Assert.That(acceleratingFactor, Is.EqualTo(accelerator));
            Assert.That(tmagnifier.GetTransformerType(), Is.TypeOf<AccelerateTransformer>());
        }

        [Test]
        public void TestFailGetAcceleratingFactor()
        {

            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const int threshold = 12;
            const uint maxYield = 20;
            const int targetNum = 30;

            TMagnifier tmagnifier = new TMagnifier(size, scaleFactor, limit, maxYield,
                new Transformer(targetNum, threshold));
            
            Assert.Throws<Exception>(() =>
            {
                double unused = tmagnifier.GetAcceleratingFactor();
            });        }

        [Test]
        public void TestSuccessfulGetModuloFactor()
        {
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            const double modulo = 10.0;
            const int targetNum = 30;
            
            TMagnifier tmagnifier = new TMagnifier(size, scaleFactor, limit, maxYield,
                new ViralTransformer(targetNum, modulo));
            double moduloFactor = tmagnifier.GetModuloFactor();

            // get moduloFactor and also verify if object is ViralTransformer
            Assert.That(moduloFactor, Is.EqualTo(modulo));
            Assert.That(tmagnifier.GetTransformerType(), Is.TypeOf<ViralTransformer>());
        }

        [Test]
        public void TestFailGetModuloFactor()
        {
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const int threshold = 12;
            const uint maxYield = 20;
            const int targetNum = 30;

            TMagnifier tmagnifier = new TMagnifier(size, scaleFactor, limit, maxYield,
                new Transformer(targetNum, threshold));
            
            Assert.Throws<Exception>(() =>
            {
                double unused = tmagnifier.GetModuloFactor();
            });
        }

        [Test]
        public void TestGetTransformerType()
        {
            
            // arbitrary values to help instantiate 3 tmagnifiers
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            const int threesHold = 12;
            const int accelerator = 4;
            const double modulo = 5.0;
            const int targetNum1 = 10;
            const int targetNum2 = 20;
            const int targetNum3 = 30;

            TMagnifier tmagnifier1 =
                new TMagnifier(size, scaleFactor, limit, maxYield, new Transformer(targetNum1, threesHold));
            TMagnifier tmagnifier2 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new AccelerateTransformer(targetNum2, accelerator));
            TMagnifier tmagnifier3 = new TMagnifier(size, scaleFactor, limit, maxYield,
                new ViralTransformer(targetNum3, modulo));
            
            // Transformer type enclosed by a Tmagnifier obvious in this test
            // but may not if only see from Tmagnifier perspective (all of them are just Tmagnifier))
            Assert.That(tmagnifier1.GetTransformerType(), Is.TypeOf<Transformer>());
            Assert.That(tmagnifier2.GetTransformerType(), Is.TypeOf<AccelerateTransformer>());
            Assert.That(tmagnifier3.GetTransformerType(), Is.TypeOf<ViralTransformer>());
            


        }
    }
}