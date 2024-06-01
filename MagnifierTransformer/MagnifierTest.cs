// Huy Quoc Nguyen
// CPSC 3200 P5 : Magnifier Transformer

using MagnifiesTransformer;

namespace MagnifierTransformer;

public class MagnifierTest
{
    [TestFixture]
    public class MagnifierUnitTest
    {
        [Test]
        public void TestSuccessfulCreateMagnifier()
        {
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            Assert.IsTrue(magnifier.GetInitialState);
            Assert.IsTrue(magnifier.IsActive);
            Assert.IsTrue(magnifier.IsUnknown);
            Assert.That(magnifier.NumQueries, Is.EqualTo(0));
        }

        [Test]
        public void TestFailCreateMagnifier()
        {
            const uint size = 10;
            const double scaleFactor = -2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            Assert.Throws<Exception>(() =>
            {
                // instantiation just for checking exception thrown purposes
                Magnifier unused = new Magnifier(size, scaleFactor, limit, maxYield);
            });
        }

        [Test]
        public void TesSuccessfulYieldSize()
        {
            const uint size = 10;
            const double scaleFactor = 2.5;
            const uint limit = 15;
            const uint maxYield = 20;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            double newSize = magnifier.YieldSize();

            Assert.IsFalse(magnifier.GetInitialState);
            Assert.IsTrue(magnifier.IsActive);
            Assert.IsTrue(magnifier.IsUp);
            Assert.That(magnifier.NumQueries, Is.EqualTo(1));
            Assert.That(newSize, Is.EqualTo(25));
            Assert.That(magnifier.MaxYield, Is.EqualTo(19));
            Assert.That(magnifier.GetInitialYield, Is.EqualTo(20));
        }

        [Test]
        public void TestFailYieldSize()
        {
            const uint size = 3;
            const double scaleFactor = 3;
            const uint limit = 10;
            const uint maxYield = 2;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            for (int i = 0; i < 2; i++)
            {
                double unused = magnifier.YieldSize();
            }

            Assert.IsFalse(magnifier.GetInitialState);
            Assert.IsTrue(magnifier.IsShutDown);
            Assert.IsTrue(magnifier.IsDown);
            Assert.That(magnifier.NumQueries, Is.EqualTo(2));
            Assert.That(magnifier.MaxYield, Is.EqualTo(0));
            Assert.That(magnifier.GetInitialYield, Is.EqualTo(2));

            Assert.Throws<Exception>(() =>
            {
                // variable is just for checking exception thrown purposes
                double unused = magnifier.YieldSize();
            });
        }

        [Test]
        public void TestReset()
        {
            const uint size = 200;
            const double scaleFactor = 100;
            const uint limit = 15;
            const uint maxYield = 20;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            for (int i = 0; i < 8; i++)
            {
                double unused = magnifier.YieldSize();
            }

            Assert.IsFalse(magnifier.GetInitialState);
            Assert.IsTrue(magnifier.IsActive);
            Assert.IsTrue(magnifier.IsDown);
            Assert.That(magnifier.NumQueries, Is.EqualTo(8));
            Assert.That(magnifier.MaxYield, Is.EqualTo(12));

            magnifier.Reset();
            Assert.IsTrue(magnifier.GetInitialState);
            Assert.That(magnifier.NumQueries, Is.EqualTo(0));
            Assert.That(magnifier.MaxYield, Is.EqualTo(20));
            Assert.IsTrue(magnifier.IsUnknown);
            Assert.IsTrue(magnifier.IsActive);
        }

        [Test]
        public void TestSuccessfulActivate()
        {
            const uint size = 200;
            const double scaleFactor = 100;
            const uint limit = 15;
            const uint maxYield = 20;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);
            magnifier.Deactivate();

            Assert.IsFalse(magnifier.GetInitialState);
            Assert.IsTrue(magnifier.IsInactive);
            Assert.IsTrue(magnifier.IsUnknown);
            Assert.That(magnifier.NumQueries, Is.EqualTo(0));
            Assert.That(magnifier.MaxYield, Is.EqualTo(20));

            magnifier.Activate();

            Assert.IsFalse(magnifier.GetInitialState);
            Assert.IsTrue(magnifier.IsActive);
            Assert.IsTrue(magnifier.IsUnknown);
            Assert.That(magnifier.NumQueries, Is.EqualTo(0));
            Assert.That(magnifier.MaxYield, Is.EqualTo(20));
        }

        [Test]
        public void TestFailActivate()
        {
            const uint size = 200;
            const double scaleFactor = 100;
            const uint limit = 15;
            const uint maxYield = 1;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);
            double unused = magnifier.YieldSize();

            Assert.Throws<Exception>(() => magnifier.Activate());
            Assert.IsTrue(magnifier.IsShutDown);
            Assert.IsTrue(magnifier.IsDown);
            Assert.That(magnifier.NumQueries, Is.EqualTo(1));
            Assert.IsFalse(magnifier.GetInitialState);
        }

        [Test]
        public void TestSuccessfulDeactivate()
        {
            const uint size = 200;
            const double scaleFactor = 100;
            const uint limit = 15;
            const uint maxYield = 20;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);
            magnifier.Deactivate();

            Assert.IsFalse(magnifier.GetInitialState);
            Assert.IsTrue(magnifier.IsInactive);
            Assert.IsTrue(magnifier.IsUnknown);
            Assert.That(magnifier.NumQueries, Is.EqualTo(0));
            Assert.That(magnifier.MaxYield, Is.EqualTo(20));
        }

        [Test]
        public void TestFailDeactivate()
        {
            const uint size = 200;
            const double scaleFactor = 100;
            const uint limit = 15;
            const uint maxYield = 1;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);
            double unused = magnifier.YieldSize();

            Assert.Throws<Exception>(() => magnifier.Deactivate());
            Assert.IsTrue(magnifier.IsShutDown);
            Assert.IsTrue(magnifier.IsDown);
            Assert.That(magnifier.NumQueries, Is.EqualTo(1));
            Assert.IsFalse(magnifier.GetInitialState);
        }

        [Test]
        public void TestSuccessfulPreCheck()
        {
            const uint size = 200;
            const double scaleFactor = 100;
            const uint limit = 15;
            const uint maxYield = 20;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            Assert.IsTrue(magnifier.IsActive);
            Assert.IsFalse(magnifier.IsShutDown);
        }

        [Test]
        public void TestFailPreCheck()
        {
            // either object is inactive or shut down
            const uint size = 20;
            const double scaleFactor = 10;
            const uint limit = 5;
            const uint maxYield = 1;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);
            magnifier.Deactivate();
            // yield in inactive mode
            Assert.IsTrue(magnifier.IsInactive);
            Assert.Throws<Exception>(() =>
            {
                double unused = magnifier.YieldSize();
            });

            magnifier.Activate();
            // yield in shut down mode
            double unused = magnifier.YieldSize();

            Assert.Throws<Exception>(() => { unused = magnifier.YieldSize(); });
            Assert.IsTrue(magnifier.IsShutDown);
            Assert.IsFalse(magnifier.IsActive);
            Assert.IsFalse(magnifier.IsInactive);
        }

        [Test]
        public void TestSuccessfulCheckDead()
        {
            const uint size = 20;
            const double scaleFactor = 10;
            const uint limit = 5;
            const uint maxYield = 3;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            for (int i = 0; i < 2; i++)
            {
                double unused = magnifier.YieldSize();
            }

            magnifier.Deactivate();
            Assert.IsTrue(magnifier.IsInactive);

            magnifier.Activate();
            Assert.IsTrue(magnifier.IsActive);

            Assert.IsFalse(magnifier.IsShutDown);
        }

        [Test]
        public void TestFailCheckDead()
        {
            const uint size = 20;
            const double scaleFactor = 10;
            const uint limit = 5;
            const uint maxYield = 5;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            for (int i = 0; i < maxYield; i++)
            {
                double unused = magnifier.YieldSize();
            }

            Assert.Throws<Exception>(() => magnifier.Deactivate());
            Assert.Throws<Exception>(() => magnifier.Activate());

            Assert.IsTrue(magnifier.IsShutDown);
            Assert.That(magnifier.NumQueries, Is.EqualTo(5));
        }

        [Test]
        public void TestChangeMaxYieldSuccessful()
        {
            const uint size = 20;
            const double scaleFactor = 10;
            const uint limit = 5;
            const uint maxYield = 5;
            const uint newMaxYield = 10;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);
            double unused = magnifier.YieldSize();
            Assert.IsFalse(magnifier.GetInitialState);
            
            Assert.That(magnifier.NumQueries, Is.EqualTo(1));
            Assert.That(magnifier.MaxYield, Is.EqualTo(4));


            magnifier.ChangeMaxYield(newMaxYield);

            Assert.That(magnifier.MaxYield, Is.EqualTo(10));
        }
        //
        [Test]
        public void TestFailChangeMaxYield()
        {
            const uint size = 20;
            const double scaleFactor = 10;
            const uint limit = 5;
            const uint maxYield = 5;
            const uint newMaxYield = 5;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);
            Assert.IsTrue(magnifier.GetInitialState);
            
            Assert.That(magnifier.NumQueries, Is.EqualTo(0));
            Assert.That(magnifier.MaxYield, Is.EqualTo(5));


            magnifier.ChangeMaxYield(newMaxYield);

            Assert.That(magnifier.MaxYield, Is.EqualTo(5));
            Assert.IsTrue(magnifier.GetInitialState);
        }

        [Test]
        public void TestToggle()
        {
            const uint size = 10;
            const double scaleFactor = 5;
            const uint limit = 6;
            const uint maxYield = 5;
            const uint newMaxYield = 10;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            double unused = magnifier.YieldSize();
            
            Assert.IsFalse(magnifier.GetInitialState);
            Assert.IsTrue(magnifier.IsActive);
            Assert.That(magnifier.MaxYield, Is.EqualTo(4));
            
            Assert.IsTrue(magnifier.IsDown);
            
            magnifier.ChangeMaxYield(newMaxYield);
            
            Assert.IsFalse(magnifier.GetInitialState);
            Assert.IsTrue(magnifier.IsActive);
            Assert.That(magnifier.MaxYield, Is.EqualTo(10));
            
            magnifier.YieldSize();

            
            Assert.IsTrue(magnifier.IsUp);
        }

        [Test]
        public void TestShutDownSuccessful()
        {
            const uint size = 10;
            const double scaleFactor = 5;
            const uint limit = 6;
            const uint maxYield = 2;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            for (int i = 0; i < maxYield; i++)
            {
                double unused = magnifier.YieldSize();
            }
            // this method is verified only if YieldSize() is successful
            Assert.IsTrue(magnifier.IsShutDown);
            Assert.IsFalse(magnifier.GetInitialState);
            Assert.That(magnifier.NumQueries, Is.EqualTo(2));
            Assert.That(magnifier.MaxYield, Is.EqualTo(0));
        }
        
        [Test]
        public void TestFailShutDown()
        {
            const uint size = 10;
            const double scaleFactor = 5;
            const uint limit = 6;
            const uint maxYield = 5;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            for (int i = 0; i < 4; i++)
            {
                double unused = magnifier.YieldSize();
            }
            // fail to shut down if there are still at least a chance to yield new size
            Assert.IsFalse(magnifier.IsShutDown);
            Assert.IsFalse(magnifier.GetInitialState);
            Assert.IsTrue(magnifier.IsActive);
            Assert.That(magnifier.NumQueries, Is.EqualTo(4));
            Assert.That(magnifier.MaxYield, Is.EqualTo(1));
        }
        
        [Test]
        public void GateCheckSuccessful()
        {
            const uint size = 10;
            const double scaleFactor = 5;
            const uint limit = 6;
            const uint maxYield = 5;
            Magnifier magnifier = new Magnifier(size, scaleFactor, limit, maxYield);

            // as long as object instantiation is successful this test will pass too => just for thorough verification
            Assert.That(magnifier.ScaleFactor, Is.GreaterThan(0));
        }
        
        [Test]
        public void TestFailGateCheck()
        {
            const uint size = 10;
            const double scaleFactor = -2;
            const uint limit = 6;
            const uint maxYield = 5;
            Assert.Throws<Exception>(() =>
            {   // just used to test if exception is thrown
                Magnifier unused = new Magnifier(size, scaleFactor, limit, maxYield);
            });
        }
    }
}