## Synopsis

ObEq (from ObjectEquality) provides basic implementation for GetHashCode and Equals methods.

## Code Example

Let's assume there is a class Sample<T> that has two fields.
It could be defined this way:

```cs
public class Sample<T>
{
    private readonly T field1;
    private readonly int field2;

    // Constructor(s)...

    // Other methods and properties...
}
```

If we need this class to implement GetHashCode() and Equals() methods, it could be easily achieved using ObEq's EqualityHelper class.

### Implementing IMemberwiseComparable

First step is to make the target type implement the `IMemberwiseComparable` interface.
This will enforce the type to define EqualityMembers property refers to all fields that should be used as equality members.

**IMPORTANT**
The referred fields should be declared as `readonly` otherwise the calculated hash code value can change every time any field value is modified.
While it's technically possible to use not `readonly` (i.e. mutable) fields, it's not recommended because it can break code that relies on object's hashcode consistency.
Dictionary is an example of a collection that will not tolerate hash code volatility.
See [Eric Lippert's blog post](https://blogs.msdn.microsoft.com/ericlippert/2011/02/28/guidelines-and-rules-for-gethashcode/) and [StackOverflow discussion](http://stackoverflow.com/questions/4718009/mutable-objects-and-hashcode) for more details.

Second step is to make the Equals() and GetHashCode() delegate result calculation to EqualityHelper class.
The simplest code will look as following:

```cs
namespace ObEq.Tests
{
    public class Sample<T> : IMemberwiseComparable
    {
        private readonly T field1;
        private readonly int field2;

        public Sample(T1 t, int i)
        {
			this.field1 = t;
			this.field2 = i;
        }

        public override bool Equals(object other)
        {
            return EqualityHelper.CalculateEquals(this, other as IMemberwiseComparable);
        }

        public override int GetHashCode()
        {
            return EqualityHelper.CalculateHashCode(EqualityMembers);
        }

        public object[] EqualityMembers => new[] { field1, field2 };
    }
}
```

### or wihtout implementing IMemberwiseComparable

Otherwise, you may write a slightly different variation of the code.
In this scenario you have more control over equality calculation.
Also, there is no need to implement the IMemberwiseComparable interface.

Notice that EqualityHelper::ReferencesEqual(object1, object2) is used to check whether the references are same or not;
whereas EqualityHelper::AllMembersEqual(equalityMembers1, equalityMembers2) is the method that does the actual fieldwise comparison for two objects.

```cs
private object[] EqualityMembers => new[] { field1, field2 };

public bool Equals(Sample<T> other)
{
	return EqualityHelper.AllMembersEqual(this.EqualityMembers, other.EqualityMembers);
}

public override bool Equals(object other)
{
	return EqualityHelper.ReferencesEqual(this, other) ??
		EqualityHelper.AllMembersEqual(this.EqualityMembers, ((Sample<T1>)other).EqualityMembers);
}

public override int GetHashCode()
{
	return EqualityHelper.CalculateHashCode(EqualityMembers);
}
```

## References

[MSDN page about GetHashCode()](https://msdn.microsoft.com/en-us/library/system.string.gethashcode(v=vs.110).aspx)

[MSDN page about Equals() method](https://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx)

## Installation

ObEq is a available in a form of a NuGet package.
Follow regular installation process to bring it to your project.
https://www.nuget.org/packages/ObEq/

## Tests

Unit tests are available in ObEq.Tests project.

## License

The code is distributed under the MIT license.

## Reporting an Issue

Reporting an issue, proposing a feature, or asking a question are all great ways to improve software quality.

Here are a few important things that package contributors will expect to see in a new born GitHub issue:
* the relevant version of the package;
* the steps to reproduce;
* the expected result;
* the observed result;
* some code samples illustrating current inconveniences and/or proposed improvements.

## Contributing

Contribution is the best way to improve any project!

1. Fork it!
2. Create your feature branch (```git checkout -b my-new-feature```).
3. Commit your changes (```git commit -am 'Added some feature'```)
4. Push to the branch (```git push origin my-new-feature```)
5. Create new Pull Request

...or follow steps described in a nice [fork guide](http://kbroman.org/github_tutorial/pages/fork.html) by Karl Broman
