using System;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Piper
{
    public class PipelineTests
    {

        public int Add10(int value)
        {
            return value + 10;
        }

        public int Minus5(int value)
        {
            return value - 5;
        }

        public int? NullIntReturn(int value)
        {
            return null;
        }

        public string ConvertToString(int value)
        {
            return value.ToString();
        }

        public class TestClass
        {
            public string Foo;
            public int Bar;
        }

        public TestClass AddFoo(TestClass test)
        {
            test.Foo = "Bar";

            return test;
        }

        public TestClass AddBar(TestClass test)
        {
            test.Bar = 1;

            return test;
        }
        
        [Test]
        public void PipeShouldTakeOneFuncAndReturnThePipelineValue()
        {
            var tenAdded = Pipeline<int>.Pipe(Add10)(10);

            Assert.AreEqual(tenAdded, 20);
        }
        
        [Test]
        public void PipeShouldTakeTwoFuncsAndReturnThePipelineValue()
        {
            var tenAddedMinus5 = Pipeline<int>.Pipe(
                Add10,
                Minus5
            )(10);
            
            Assert.AreEqual(tenAddedMinus5, 15);
        }
        
        [Test]
        public void PipeShouldPipeAClassThroughAPipeline()
        {
            var builtTest = Pipeline<TestClass>.Pipe(
                AddFoo,
                AddBar
            )(new TestClass());
            
            Assert.AreEqual(builtTest.Foo, "Bar");
            Assert.AreEqual(builtTest.Bar, 1);
        }

        [Test]
        public void IfNotNullOverloadShouldFireCallbackIfPropertyOnObjectIsNull()
        {
            var testNoBar = Pipeline<TestClass>.Pipe(
                Pipeline<TestClass>.IfNotNullProp("Fizz", AddFoo),
                AddBar
            )(new TestClass());
            
            Assert.IsNull(testNoBar.Foo);
            Assert.AreEqual(testNoBar.Bar, 1);
        }
    }
}