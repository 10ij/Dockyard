# What is going on here???

`DatabaseMigrator.cs` uses DbUp to automatically make sure new migrations are done

In the PHP app, I used to just kind of not handle migrations, except for a few times where I'd write a migrator script, but I can't remmeber if I put those on Git

Well now migrations are handled, just make your changes in a new file with the number prefix incremented

DbUp will check against current schema and auto-migrate, hopefully

Aside from DbUp, `DatabaseMigrator.cs` does create the SQLite strings, using the builder, which I think in the PHP app was done manually through my own concatanation

And then it passes on the DB to DbUp

Also compared the PHP app, no more raw SQL in my code files! The app reads seperate SQL files

`Connection.cs` does the work that `db.php` would've done, but not in PHP, in C#, innovation

## 001_InitialSchema.sql

This file differs a bit to the PHP one, so its snake_case now, as i've learnt it should be, postgres doesn't even by default store uppercase letters unless you quote them

Some tables used to have lowercase anyways like `users` so theres really no issue in it, just making it more standardised

As for Dapper, it is apparently case-insenstive, and there is a config option to ignore underscores, so even if a have an object `AuditLogs` it should still map it to `audit_logs`

`usernames` are no longer unique, since we rely on OIDC sub ID's anyways, username is just for show, literally just to greet the user

Added SQL checks, at a basic level, for invalid data

Now uses standard UTC timestamps

`apps` table has JSON support now, so ports dont have to be broken down, and rebuilt, and comma seperated, and whatever like it was in PHP, now its just straight JSON data that can be easily read by me, and machine

`container_permissions` links by name not ID, names are unique anyways, idk its not much of a difference 

Added Write Ahead Logging