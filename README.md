# Should.Extensions
A collection of useful extensions to the Should Assertion Library for testing different types and enumerables of those type for equality. Like the Should extensions themselves, these extensions provide assertions only, and therefore are also test runner agnostic.  I've been using these extensions in projects for several years and thought they would be useful for others.

* [ShouldHaveSamePropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)](#ShouldHaveSamePropertyValues)
* [ShouldHaveSameSharedPropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)](#ShouldHaveSameSharedPropertyValues)
* [ShouldHaveSameProperties<TActual, TExpected>(this TActual actual, TExpected expected)](#ShouldHaveSameProperties)

##### <a name="ShouldHaveSamePropertyValues"></a>ShouldHaveSamePropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)

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

##### <a name="ShouldHaveSameSharedPropertyValues"></a>ShouldHaveSameSharedPropertyValues<TActual, TExpected>(this TActual actual, TExpected expected)

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

##### <a name="ShouldHaveSameProperties"></a>ShouldHaveSameProperties<TActual, TExpected>(this TActual actual, TExpected expected)

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
