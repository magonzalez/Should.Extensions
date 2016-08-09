# Should.Extensions
A collection of useful extensions to the Should Assertion Library for testing different types and enumerables of those type for equality. Like the Should extensions themselves, these extensions provide assertions only, and therefore are also test runner agnostic.  I've been using these extensions in projects for several years and thought they would be useful for others.

* [Assertions](#Assertions)
    * [Complex Type](#ComplexType)
        * [ShouldHaveSamePropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)](#ShouldHaveSamePropertyValues)
        * [ShouldHaveSameSharedPropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)](#ShouldHaveSameSharedPropertyValues)
        * [ShouldHaveSameProperties<TActual, TExpected>(this TActual actual, TExpected expected)](#ShouldHaveSameProperties)
    * [IEnumerable&lt;Complex Type&gt;](#IEnumerableOFComplexType)
        * [ShouldHaveSimilarItems<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected)](#ShouldHaveSimilarItems)
        * [ShouldHaveSameItems<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected)](#ShouldHaveSameItems)
        * [ShouldHaveSameItems<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected, Func<TActual, TExpected, bool> equalityTester)](#ShouldHaveSameItemsWithEqualityTester)
        * [ShouldHaveSameValueItems<T>(this IEnumerable<T> actual, IEnumerable<T> expected) where T : struct](#ShouldHaveSameValueItems)
    * [Misc.](#Misc)
        * [ShouldBeNullIfExpectingNull<TActual, TExpected>(this TActual actual, TExpected expected, string errorMessage = "")](#ShouldBeNullIfExpectingNull)
        * [ShouldBeNullIfExpectingNull<TActual, TExpected>(this TActual? actual, TExpected? expected, string errorMessage = "")](#ShouldBeNullIfExpectingNullNullable)
        * [ShouldEqualDateTime(this DateTime? actual, DateTime? expected, string errorMessage = "")](#ShouldEqualDateNullable)
        * [ShouldEqualDateTime(this DateTime actual, DateTime expected, string errorMessage = "")](#ShouldEqualDate)
        * [ShouldEqualDate(this DateTime actual, DateTime expected, string errorMessage = "")](#ShouldEqualDate)
        * [ShouldEqualTime(this DateTime actual, DateTime expected, string errorMessage = "")](#ShouldEqualTime)
* [Utility Methods](#UtilityMethods)
    * [HasSameSharedPropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)](#HasSameSharedPropertyValues)
    * [HasSamePropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)](#HasSamePropertyValues)
    * [HasSameProperties<TActual, TExpected>(this TActual actual, TExpected expected)](#HasSameProperties)

## <a name="Assertions"></a>Assertions

### <a name="ComplexType"></a>Complex Type
This set of methods can be used to compare the properties of two complex types for equality.

#### <a name="ShouldHaveSamePropertyValues"></a>ShouldHaveSamePropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)

Verifies that the *actual* instance has the same property values as the *expected* instance. 

```c#
var expected = new Person {
    Id = 123,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

var actual = new Person {
    Id = 123,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

// Passes - all property values are the same.
actual.ShouldHaveSamePropertyValues(expected);

actual = new Person {
    Id = 9999,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
}

// Fails - Id values are different.
actual.ShouldHaveSamePropertyValues(expected);
```
This method doesn't care if the *actual* and *expected* are the same types. In fact, they don't have to be related in any way. The method uses reflection to compare the two instances and determine if they have the same values. 

```c#
var expected = new Person {
    Id = 123,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

var actual = new Dude {
    Id = 123,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

// Passes - all property values are the same.
actual.ShouldHaveSamePropertyValues(expected);

actual = new SomeOtherDude {
    Id = 9999,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
}

// Fails - Id values are different.
actual.ShouldHaveSamePropertyValues(expected);
```
This method does care if the two instance have the same of properties though.

```c#
public class Person {
    public long Id { get; set; }
    public Gender Gender { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
}

public class Dude {
    public long Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
}

var expected = new Person {
    Id = 123,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

var actual = new Dude {
    Id = 123,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

// Fails - actual doesn't have a Gender property.
actual.ShouldHaveSamePropertyValues(expected);
```

The instances are all checked for null value equality. That is, if the *expected* instance is null, the *actual* instance is expected to be null. If the *expected* instance in not null, the *actual* instance is expected not to be null as well.

```c#

var expected = new Person {
    Id = 123,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

var actual = null;

// Fails - expected is not null, but actual is.
actual.ShouldHaveSamePropertyValues(expected);

expected = null;

var actual = new Person {
    Id = 123,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

// Fails - expected is null, but actual is not.
actual.ShouldHaveSamePropertyValues(expected);

expected = null;
actual = null;

// PAsses - both expected and actual are null.
actual.ShouldHaveSamePropertyValues(expected);
```

#### <a name="ShouldHaveSameSharedPropertyValues"></a>ShouldHaveSameSharedPropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)

This method is very similar to **ShouldHaveSamePropertyValues** except that it only checks the values of properties that both instances share.

```c#
public class Person {
    public long Id { get; set; }
    public Gender Gender { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
}

public class Dude {
    public long Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
}

var expected = new Person {
    Id = 123,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

var actual = new Dude {
    Id = 123,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

// Passes - All shared properties have the same values.
actual.ShouldHaveSameSharedPropertyValues(expected);
```

#### <a name="ShouldHaveSameProperties"></a>ShouldHaveSameProperties<TActual, TExpected>(this TActual actual, TExpected expected)

This method can be used to validate that two instances have the same propertie values.

```c#
public class Person {
    public long Id { get; set; }
    public Gender Gender { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
}

public class Man {
    public long Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
}

public class Dude {
    public long Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
}

var person = new Person {
    Id = 123,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

var dude = new Dude {
    Id = 123,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

// Fails - dude doesn't have a Gender property.
dude.ShouldHaveSameProperties(person);

var man = new Man {
    Id = 345,
    Name = "John Smith",
    Age = 32,
    BirthDate = new DataTime(1951, 8, 20)
};

// Fails - dude and man have the same properties.
dude.ShouldHaveSameProperties(man);
```

### <a name="IEnumerableOFComplexType"></a>IEnumerable&lt;Complex Type&gt;
This set of methods can be used to compare the contents of IEnumerable<T> instances for equality.

#### <a name="ShouldHaveSimilarItems"></a>ShouldHaveSimilarItems<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected)

Verifies that the *actual* instance has the same items as the *expected* instance using the [HasSameSharedPropertyValues](#HasSameSharedPropertyValues) utility method to determine if two items are equal. That is, it only validates that the shared properties are equal between two items.

#### <a name="ShouldHaveSameItems"></a>ShouldHaveSameItems<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected)

Verifies that the *actual* instance has the same items as the *expected* instance using the [HasSamePropertyValues](#HasSamePropertyValues) utility method to determine if two items are equal. That is, it validates that all properties are equal between two items.

#### <a name="ShouldHaveSameItemsWithEqualityTester"></a>ShouldHaveSameItems<TActual, TExpected>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected, Func<TActual, TExpected, bool> equalityTester)

Verifies that the *actual* instance has the same items as the *expected* instance using the given **Func&lt;TActual, TExpected, bool&gt;** to determine if two items are equal.

#### <a name="ShouldHaveSameValueItems"></a>ShouldHaveSameValueItems<T>(this IEnumerable<T> actual, IEnumerable<T> expected) where T : struct

Verifies that the *actual* instance has the same **ValueType** items as the *expected* instance. That is, it null checks the two enumerables, compares their counts and that every item in the expected instance is also in the actual instance.

```c#
public struct Person {
    public long Id { get; set; }
    public Gender Gender { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
}

var person = new Person1 {
    Id = 1,
    Gender = Gender.Male,
    Name = "John Doe",
    Age = 40,
    BirthDate = new DataTime(1951, 8, 20)
};

var person = new Person2 {
    Id = 2,
    Gender = Gender.Female,
    Name = "Jane Doe",
    Age = 21,
    BirthDate = new DataTime(1970, 8, 20)
};

var person = new Person3 {
    Id = 3,
    Gender = Gender.Female,
    Name = "Janet Doe",
    Age = 30,
    BirthDate = new DataTime(1961, 8, 20)
};

List<Person> group_1_1 = new List<Person> { Person1, Person2 };
List<Person> group_1_2 = new List<Person> { Person2, Person3 };
List<Person> group_1_3 = new List<Person> { Person1 };
List<Person> group_1_4 = new List<Person> { Person3 };
List<Person> group_1_5 = new List<Person> { Person3 };
List<Person> group_1_5 = new List<Person>();
List<Person> group_1_6 = null;

// Fail
group_1_1.ShouldHaveSameValueItems(group_1_2);
group_1_1.ShouldHaveSameValueItems(group_1_3);
group_1_1.ShouldHaveSameValueItems(group_1_4);
group_1_1.ShouldHaveSameValueItems(group_1_5);
group_1_1.ShouldHaveSameValueItems(group_1_6);

List<Person> group_2_1 = new List<Person> { Person1, Person2 };
List<Person> group_2_2 = new List<Person> { Person2, Person3 };
List<Person> group_2_3 = new List<Person> { Person1 };
List<Person> group_2_4 = new List<Person> { Person3 };
List<Person> group_2_5 = new List<Person> { Person3 };
List<Person> group_2_5 = new List<Person>();
List<Person> group_2_6 = null;

// Pass
group_1_1.ShouldHaveSameValueItems(group_2_1);
group_1_2.ShouldHaveSameValueItems(group_2_2);
group_1_3.ShouldHaveSameValueItems(group_2_3);
group_1_4.ShouldHaveSameValueItems(group_2_4);
group_1_5.ShouldHaveSameValueItems(group_2_5);
group_1_6.ShouldHaveSameValueItems(group_2_6);

```

### <a name="Misc"></a>Misc.
This set of methods are used to implement the other assertion methods in this library. These methods are more specific assertions that can be used in your own code.

#### <a name="ShouldBeNullIfExpectingNull"></a>ShouldBeNullIfExpectingNull<TActual, TExpected>(this TActual actual, TExpected expected, string errorMessage = "")

#### <a name="ShouldBeNullIfExpectingNullNullable"></a>ShouldBeNullIfExpectingNull<TActual, TExpected>(this TActual? actual, TExpected? expected, string errorMessage = "")

#### <a name="ShouldEqualDateTimeNullable"></a>ShouldEqualDateTime(this DateTime? actual, DateTime? expected, string errorMessage = "")

#### <a name="ShouldEqualDateTime"></a>ShouldEqualDateTime(this DateTime actual, DateTime expected, string errorMessage = "")

#### <a name="ShouldEqualDate"></a>ShouldEqualDate(this DateTime actual, DateTime expected, string errorMessage = "")

#### <a name="ShouldEqualTime"></a>ShouldEqualTime(this DateTime actual, DateTime expected, string errorMessage = "")

## <a name="UtilityMethods"></a>Utility Methods

#### <a name="HasSameSharedPropertyValues"></a>HasSameSharedPropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)

#### <a name="HasSamePropertyValues"></a>HasSamePropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)

#### <a name="HasSameProperties"></a>HasSameProperties<TActual, TExpected>(this TActual actual, TExpected expected)