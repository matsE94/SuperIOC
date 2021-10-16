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
            var provider = container.BuildProvider();
            //act
            var guidCreator = provider.Get<IBatmanService>();

            //assert
            guidCreator.Should().NotBeNull();
        }

        [Fact]
        public void Get_Singleton_ShouldReturnSameObject()
        {
            //arrange
            var container = new SuperContainer();
            container.RegisterSingleton<IBatmanService, BatmanService>();
            var provider = container.BuildProvider();
            //act
            var instance1 = provider.Get<IBatmanService>();
            var instance2 = provider.Get<IBatmanService>();

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
            var provider = container.BuildProvider();
            //act
            var instance1 = provider.Get<BatmanController>();
            var instance2 = provider.Get<BatmanController>();

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
            var provider = container.BuildProvider();
            //act
            var instance1 = provider.Get<IBatmanService>();
            var instance2 = provider.Get<IBatmanService>();

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
            container.RegisterTransient<BatmanController, BatmanController>();
            container.RegisterTransient<IBatmanService, BatmanService>();
            var provider = container.BuildProvider();


            //act
            var instance1 = provider.Get<BatmanController>();
            var instance2 = provider.Get<BatmanController>();

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
            var provider = container.BuildProvider();

            //act
            var controller = provider.Get<BatmanController>();

            //assert
            controller.Should().NotBeNull(); // means we successfully activated nested dependencies
        }

        class A
        {
        }

        class B
        {
            private readonly A a;

            public B(A a)
            {
                this.a = a;
            }
        }

        class C
        {
            private readonly B b;

            public C(B b)
            {
                this.b = b;
            }
        }

        [Fact]
        public void Get_WithDoubleNestedDI_ShouldNotReturnNull()
        {
            //arrange
            var container = new SuperContainer();
            container.RegisterTransient<A>();
            container.RegisterTransient<B>();
            container.RegisterSingleton<C>();
            var provider = container.BuildProvider();
            //act
            var result = provider.Get<C>();

            //assert
            result.Should().NotBeNull(); // means we successfully activated nested dependencies
        }

        class PersonController
        {
            private readonly AutoMapper _mapper;
            private readonly Mediator _mediator;

            public PersonController(AutoMapper mapper, Mediator mediator)
            {
                _mapper = mapper;
                _mediator = mediator;
            }
        }

        class AutoMapper
        {
        }

        class Mediator
        {
        }

        [Fact]
        public void Get_WithMultipleDI_ShouldNotReturnNull()
        {
            //arrange
            var container = new SuperContainer();
            container.RegisterTransient<PersonController>();
            container.RegisterTransient<AutoMapper>();
            container.RegisterSingleton<Mediator>();
            var provider = container.BuildProvider();
            //act
            var result = provider.Get<PersonController>();

            //assert
            result.Should().NotBeNull(); // means we successfully activated nested dependencies
        }

        [Fact]
        public void GetOrThrow_WithNoRegistrations_ShouldThrowExactly()
        {
            //arrange
            var container = new SuperContainer();
            var provider = container.BuildProvider();

            //act+assert
            provider.Invoking(x => x.GetOrThrow<BatmanService>())
                .Should()
                .ThrowExactly<SuperContainerException>();
        }

        [Fact]
        public void Custom_creator()
        {
            //arrange
            var container = new SuperContainer();
            var provider = container.BuildProvider();

            //act
            container.RegisterTransient<IBatmanService,BatmanService>();
            container.RegisterTransient<BatmanController>(
                p =>
                {
                    var service = p.Get<IBatmanService>();
                    return new BatmanController(service);
                });

            provider.Get<BatmanController>();
        }
    }
}