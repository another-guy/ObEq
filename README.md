## Synopsis

Ob[ject]Eq[uality] provides basic implementation for GetHashCode and Equals methods.

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

First, the EqualityMembers property refers to all fields that should be used as equality members.

**IMPORTANT**
The referred fields should be declared as `readonly` otherwise the calculated hash code value can change every time any field value is modified.
While it's technically possible to use not `readonly` (i.e. mutable) fields, it's not recommended because it can break code that relies on object's hashcode consistency.
Dictionary is an example of a collection that will not tolerate hash code volatility.
See [Eric Lippert's blog post](https://blogs.msdn.microsoft.com/ericlippert/2011/02/28/guidelines-and-rules-for-gethashcode/) and [StackOverflow discussion](http://stackoverflow.com/questions/4718009/mutable-objects-and-hashcode) for more details.

Second, the Equals() and GetHashCode() delegate result calculation to EqualityHelper by passing it the EqualityMembers as an argument.

Notice that EqualityHelper::CalculateReferentialEquals(object1, object2) is used to check whether the references are same or not;
whereas EqualityHelper::CalculateEquals(equalityMembers1. equalityMembers2) is the method that does the actual fieldwise comparison for two objects.

```cs
private object[] EqualityMembers => new[] { field1, field2 };

protected bool Equals(Sample<T> other)
{
	return EqualityHelper.CalculateEquals(this.EqualityMembers, other.EqualityMembers);
}

public override bool Equals(object other)
{
	var referenceEqualityResult = EqualityHelper.CalculateReferentialEquals(this, other);
	return referenceEqualityResult ??
		EqualityHelper.CalculateEquals(this.EqualityMembers, ((Sample<T1>)other).EqualityMembers);
}

public override int GetHashCode()
{
	return EqualityHelper.CalculateHashCode(EqualityMembers);
}
```

## References

MSDN page about hash code: https://msdn.microsoft.com/en-us/library/system.string.gethashcode(v=vs.110).aspx
MSDN page about equals: https://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx

## Installation

ObEq is a available in a form of a NuGet package.
Follow regular installation process to bring it to your project.
https://www.nuget.org/packages/ObEq/

## Tests

Unit tests are available in ObEq.Tests project.

## License

The code is distributed under the MIT license.

## Contributing

Contribution is the best way to improve any project!

1. Fork it!
2. Create your feature branch (```git checkout -b my-new-feature```).
3. Commit your changes (```git commit -am 'Added some feature'```)
4. Push to the branch (```git push origin my-new-feature```)
5. Create new Pull Request

...or follow steps described in a nice [fork guide](http://kbroman.org/github_tutorial/pages/fork.html) by Karl Broman
