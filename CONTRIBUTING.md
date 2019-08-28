Create a new branch following `gitflow` pattern -> commit your changes -> pull request to add changes to repository.

Please follow code style if possible, you can pull request changes in your own coding style but be aware that I will convert them to follow these guidelines.
  
  `ClassName`
  
  `StructName`
  
  `Generics<T>` is allowed. But when possible description is encouraged: `Generics<TData>`.
  
  ---
  
  `PublicField`
  
  `protectedField`
  
  `_privateField`
  
  ---
  
  `CONSTANT_FIELD`
  
  `_PRIVATE_CONSTANT_FIELD`
  
  `s_privateStaticField`
  
  `S_PublicStaticField`
  
  ---
  
  `PublicProperty { get; set; }`
  
  `_PublicPropertyWithGetOnly ({ get; } | =>)`
  
  `PublicPropertyWithEncapsulatedSet_ { get; (private | protected) set; }`
  
  ---
  
  `Method() {}`
  
  `StaticMethod() {}`
