using FluentAssertions;
using SuperIOC.SuperContainer.Test.DummyClasses;
using Xunit;

namespace SuperIOC.SuperContainer.Test
{
    public class SuperContainerTests
    {
        [Fact]
        public void Get_Transient_ShouldNotReturnNull()
        {
            //arrange
            var container = new SuperContainer();
            container.RegisterTransient<IBatmanService, BatmanService>();

            //act
            var guidCreator = container.Get<IBatmanService>();

            //assert
            guidCreator.Should().NotBeNull();
        }

        [Fact]
        public void Get_Singleton_ShouldReturnSameObject()
        {
            //arrange
            var container = new SuperContainer();
            container.RegisterSingleton<IBatmanService, BatmanService>();

            //act
            var instance1 = container.Get<IBatmanService>();
            var instance2 = container.Get<IBatmanService>();

            //assert
            var hashcode1 = instance1.GetHashCode();
            var hashcode2 = instance2.GetHashCode();

            hashcode1.Should().Be(hashcode2);
        }

        [Fact]
        public void Get_NestedSingleton_ShouldReturnSameObject()
        {
            //arrange
            var container = new SuperContainer();
            container.RegisterSingleton<BatmanController, BatmanController>();
            container.RegisterSingleton<IBatmanService, BatmanService>();
            
            //act
            var instance1 = container.Get<BatmanController>();
            var instance2 = container.Get<BatmanController>();
            
            //assert
            var hashcode1 = instance1.GetHashCode();
            var hashcode2 = instance2.GetHashCode();

            hashcode1.Should().Be(hashcode2);
        }

        [Fact]
        public void Get_Transient_ShouldReturnNewObject()
        {
            //arrange
            var container = new SuperContainer();
            container.RegisterTransient<IBatmanService, BatmanService>();

            //act
            var instance1 = container.Get<IBatmanService>();
            var instance2 = container.Get<IBatmanService>();

            //assert
            var hashcode1 = instance1.GetHashCode();
            var hashcode2 = instance2.GetHashCode();

            hashcode1.Should().NotBe(hashcode2);
        }
        
        [Fact]
        public void Get_NestedTransient_ShouldReturnNewObject()
        {
            //arrange
            var container = new SuperContainer();
            container.RegisterTransient<BatmanController, BatmanController>(); ;
            container.RegisterTransient<IBatmanService, BatmanService>();
            
            //act
            var instance1 = container.Get<BatmanController>();
            var instance2 = container.Get<BatmanController>();
            
            //assert
            var hashcode1 = instance1.GetHashCode();
            var hashcode2 = instance2.GetHashCode();

            hashcode1.Should().NotBe(hashcode2);
        }

        [Fact]
        public void Get_WithNestedDI_ShouldNotReturnNull()
        {
            //arrange
            var container = new SuperContainer();
            container.RegisterTransient<IBatmanService, BatmanService>();
            container.RegisterSingleton<BatmanController, BatmanController>();

            //act
            var controller = container.Get<BatmanController>();

            //assert
            controller.Should().NotBeNull(); // means we successfully activated nested dependencies
        }

        [Fact]
        public void GetOrThrow_WithNoRegistrations_ShouldThrowExactly()
        {
            //arrange
            var container = new SuperContainer();

            //act
            container.Invoking(x => x.GetOrThrow<BatmanService>())
                .Should()
                .ThrowExactly<SuperContainerException>();
        }
        
    }
}