#region Usings

using System.Linq;
using Ninject.Dynamic;
using Ninject.Tests.Fakes;
using Xunit;
using Xunit.Should;

#endregion

namespace Ninject.Tests.Integration.DlrKernelTests
{
    public class DlrKernelContext
    {
        protected readonly DlrKernel kernel;

        public DlrKernelContext()
        {
            kernel = new DlrKernel();
            kernel.LoadAssemblies(typeof (DlrKernelContext));
        }
    }

    public class WhenConfiguredFromCSharp : DlrKernelContext
    {
        [Fact]
        public void SingleInstanceIsReturnedWhenOneBindingIsRegistered()
        {
            kernel.Bind<IWeapon>().To<Sword>();

            var weapon = kernel.Get<IWeapon>();

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Sword>();
        }

        [Fact]
        public void FirstInstanceIsReturnedWhenMultipleBindingsAreRegistered()
        {
            kernel.Bind<IWeapon>().To<Sword>();
            kernel.Bind<IWeapon>().To<Shuriken>();

            var weapon = kernel.Get<IWeapon>();

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Sword>();
        }

        [Fact]
        public void DependenciesAreInjectedViaConstructor()
        {
            kernel.Bind<IWeapon>().To<Sword>();
            kernel.Bind<IWarrior>().To<Samurai>();

            var warrior = kernel.Get<IWarrior>();

            warrior.ShouldNotBeNull();
            warrior.ShouldBeInstanceOf<Samurai>();
            warrior.Weapon.ShouldNotBeNull();
            warrior.Weapon.ShouldBeInstanceOf<Sword>();
        }
    }


    public class WhenConfiguredFromRuby : DlrKernelContext
    {
        [Fact]
        public void SingleInstanceIsReturnedWhenOneBindingIsRegistered()
        {
            kernel.Load("ruby/config_single.rb");

            var weapon = kernel.Get<IWeapon>();

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Sword>();
        }

        [Fact]
        public void FirstInstanceIsReturnedWhenMultipleBindingsAreRegistered()
        {
            kernel.Load("ruby/config_double.rb");

            var weapon = kernel.Get<IWeapon>();

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Sword>();
        }

        [Fact]
        public void DependenciesAreInjectedViaConstructor()
        {
            kernel.Load("ruby/config_two_types.rb");

            var warrior = kernel.Get<IWarrior>();

            warrior.ShouldNotBeNull();
            warrior.ShouldBeInstanceOf<Samurai>();
            warrior.Weapon.ShouldNotBeNull();
            warrior.Weapon.ShouldBeInstanceOf<Sword>();
        }
    }

    public class WhenBoundToSelf : DlrKernelContext
    {
        [Fact]
        public void SingleInstanceIsReturnedWhenOneBindingIsRegistered()
        {
            kernel.Load("ruby/config_to_self.rb");

            var weapon = kernel.Get<Sword>();

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Sword>();
        }


        [Fact]
        public void DependenciesAreInjectedViaConstructor()
        {
            kernel.Load("ruby/config_two_types_to_self.rb");

            var samurai = kernel.Get<Samurai>();

            samurai.ShouldNotBeNull();
            samurai.Weapon.ShouldNotBeNull();
            samurai.Weapon.ShouldBeInstanceOf<Sword>();
        }
    }


    public class WhenBoundToGenericServiceRegisteredViaOpenGenericType : DlrKernelContext
    {
        [Fact]
        public void GenericParametersAreInferred()
        {
            kernel.Load("ruby/config_open_generics.rb");

            var services = kernel.GetAll<IGeneric<int>>().ToArray();

            services.ShouldNotBeNull();
            services.Length.ShouldBe(2);
            services[0].ShouldBeInstanceOf<GenericService<int>>();
            services[1].ShouldBeInstanceOf<GenericService2<int>>();
        }
    }

    public class WhenBoundWithConstraints : DlrKernelContext
    {
        [Fact]
        public void ReturnsServiceRegisteredViaBindingWithSpecifiedName()
        {
            kernel.Load("ruby/config_named.rb");

            var weapon = kernel.Get<IWeapon>("sword");

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Sword>();
        }

        [Fact]
        public void ReturnsServiceRegisteredViaBindingThatMatchesPredicate()
        {
            kernel.Load("ruby/config_metadata.rb");

            var weapon = kernel.Get<IWeapon>(x => x.Get<string>("type") == "melee");

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Sword>();
        }
    }

    public class WhenBoundWithConstructorArguments : DlrKernelContext
    {
        [Fact]
        public void ReturnsServiceRegistered()
        {
            kernel.Load("ruby/config_constructor_arguments.rb");

            var weapon = kernel.Get<IWeapon>();

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Knife>();
            weapon.Name.ShouldBe("Blunt knife");
        }

        [Fact]
        public void ReturnsServiceWhenRegisteredAsDSL()
        {
            kernel.Load("ruby/config_constructor_arguments_dsl.rb");

            var weapon = kernel.Get<IWeapon>();

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Knife>();
            weapon.Name.ShouldBe("Blunt knife");
        }
    }

    public class WhenBoundWithWhenArgument : DlrKernelContext
    {
        [Fact]
        public void ResolvesTheCorrectTypeAccordingToCondition()
        {
            kernel.Load("ruby/config_when.rb");

            var weapon = kernel.Get<IWeapon>();
            var warrior = kernel.Get<IWarrior>();

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Sword>();
            warrior.ShouldNotBeNull();
            warrior.Weapon.ShouldBeInstanceOf<Shuriken>();
        }

        [Fact]
        public void ResolvesTheCorrectTypeAccordingToConditionWithDsl()
        {
            kernel.Load("ruby/config_when_dsl.rb");

            var weapon = kernel.Get<IWeapon>();
            var warrior = kernel.Get<IWarrior>();

            weapon.ShouldNotBeNull();
            weapon.ShouldBeInstanceOf<Sword>();
            warrior.ShouldNotBeNull();
            warrior.Weapon.ShouldBeInstanceOf<Shuriken>();
        }
    }
}