# Introduction 
This is a repository for one or more closely-related NuGet packages. A library containing classes for common uses in logging and tracing, metrics collection, across any .NET project.

# Getting Started
There's nothing to it:
1.	Reference the package
2.	Use types the new types in namespaces under the Evoq root
3.	Profit!

# Build and Test
Please ensure that any and all classes you add to this package are very well tested. This is a general package intended for use across many projects. Code must be of the highest quality. 

# Contribute
Add any types pertaining to instrumentation in the same vein as the types already in the package. When adding code that adds a non-framework dependency, create a new, more specialised library project and NuGet package that takes the dependency.

For example, a class that needs to convert JSON objects should have the conversion abstracted using a class that uses only basic types, then an optional specialized implementation that uses Json.NET can be placed in a new package that takes the Newtonsoft.Json dependency. This way, consumers never have to pull-in dependencies to packages for types that they don't intend to use.

Remember to update the .nuspec file(s) as needed.