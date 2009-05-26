
include Ninject::Tests::Fakes

# IGeneric is a generic interface and GenericService is a generic type
# we don't have to specify any special notation for open generics

to_configure_ninject do |ninject|
  ninject.bind IGeneric, :to => GenericService
  ninject.bind IGeneric, :to => GenericService2
end