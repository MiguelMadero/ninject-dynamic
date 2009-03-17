
include Ninject::Tests::Fakes

to_configure_ninject do |ninject|
  ninject.bind IWeapon, :to => Sword
end