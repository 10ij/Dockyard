# What is going on here???

Simple, `Schema.cs` contains POCOs based on the SQL schema

The PHP app would've outright not even had models, barely even any classes in general tbf, it'd just shove the output from SQLite into a list and call it a day rather than return a list of objects

These are in PascalCase but should be fine since Dapper can map to snake_case anyways