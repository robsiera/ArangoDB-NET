# ArangoDB-NET

ArangoDB-NET is a C# driver for [ArangoDB](https://www.arangodb.com/) NoSQL multi-model database. Driver implements and communicates with database backend through its [HTTP API](https://docs.arangodb.com/HttpApi/README.html) interface and runs on Microsoft .NET and mono framework.

## Docs contents

- [Installation](#installation)
- [Connection management](#connection-management)
- [Basic usage](docs/BasicUsage.md)
  - [Connection management](docs/BasicUsage.md#connection-management)
  - [Database context](docs/BasicUsage.md#database-context)
  - [ArangoResult object](docs/BasicUsage.md#arangoresult-object)
  - [ArangoError object](docs/BasicUsage.md#arangoerror-object)
  - [JSON representation](docs/BasicUsage.md#json-representation)
  - [Fluent API](docs/BasicUsage.md#fluent-api)
- [Database operations](docs/DatabaseOperations.md)
  - [Create database](docs/DatabaseOperations.md#create-database)
  - [Retrieve current database](docs/DatabaseOperations.md#retrieve-current-database)
  - [Retrieve accessible databases](docs/DatabaseOperations.md#retrieve-accessible-databases)
  - [Retrieve all databases](docs/DatabaseOperations.md#retrieve-all-databases)
  - [Retrieve database collections](docs/DatabaseOperations.md#retrieve-database-collections)
  - [Delete database](docs/DatabaseOperations.md#delete-database)
- [Collection operations](#collection-operations)
  - [Create collection](#create-collection)
  - [Retrieve collection](#retrieve-collection)
  - [Retrieve collection properties](#retrieve-collection-properties)
  - [Retrieve collection count](#retrieve-collection-count)
  - [Retrieve collection figures](#retrieve-collection-figures)
  - [Retrieve collection revision](#retrieve-collection-revision)
  - [Retrieve collection checksum](#retrieve-collection-checksum)
  - [Retrieve all documents](#retrieve-all-documents)
  - [Retrieve all edges](#retrieve-all-edges)
  - [Truncate collection](#truncate-collection)
  - [Load collection](#load-collection)
  - [Unload collection](#unload-collection)
  - [Change collection properties](#change-collection-properties)
  - [Rename collection](#rename-collection)
  - [Rotate collection journal](#rotate-collection-journal)
  - [Delete collection](#delete-collection)
- [Document operations]()
- [Edge operations]()
- [AQL query cursors execution]()
- [AQL user functions management]()

## Installation

There are following ways to install the driver:

- download and install [nuget package]() which contains latest stable version
- clone ArangoDB-NET [repository](https://github.com/yojimbo87/ArangoDB-NET) and build [master branch](https://github.com/yojimbo87/ArangoDB-NET/tree/master) to have latest stable version or [devel branch](https://github.com/yojimbo87/ArangoDB-NET/tree/devel) to have latest experimental version

## Collection operations

Collection operations are focused on management of document and edge type collections which are accessible through `Collection` property in database context object.

### Create collection

Creates new collection in current database context.

Applicable optional parameters available through fluent API:

- `Type(ArangoCollectionType value)` - Determines type of the collection. Default value: Document.
- `WaitForSync(bool value)` - Determines whether to wait until data are synchronised to disk. Default value: false.
- `JournalSize(long value)` - Determines maximum size of a journal or datafile in bytes. Default value: server configured.
- `DoCompact(bool value)` - Determines whether the collection will be compacted. Default value: true.
- `IsSystem(bool value)` - Determines whether the collection is a system collection. Default value: false.
- `IsVolatile(bool value)` - Determines whether the collection data is kept in-memory only and not made persistent. Default value: false.
- `KeyGeneratorType(ArangoKeyGeneratorType value)` - Determines the type of the key generator. Default value: Traditional.
- `AllowUserKeys(bool value)` - Determines whether it is allowed to supply custom key values in the _key attribute of a document.
- `KeyIncrement(long value)` - Determines increment value for autoincrement key generator.
- `KeyOffset(long value)` - Determines initial offset value for autoincrement key generator.
- `NumberOfShards(int value)` - Determines the number of shards to create for the collection in cluster environment. Default value: 1.
- `ShardKeys(List<string> value)` - Determines which document attributes are used to specify the target shard for documents in cluster environment. Default value: ["_key"].

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

// creates new document type collection
var createCollectionResult = db.Collection
    .KeyGeneratorType(ArangoKeyGeneratorType.Autoincrement)
    .WaitForSync(true)
    .Create("MyDocumentCollection");
    
if (createCollectionResult.Success)
{
    var id = createResult.Value.String("id");
    var name = createResult.Value.String("name");
    var waitForSync = createResult.Value.Bool("waitForSync");
    var isVolatile = createResult.Value.Bool("isVolatile");
    var isSystem = createResult.Value.Bool("isSystem");
    var status = createResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = createResult.Value.Enum<ArangoCollectionType>("type");
}
```

### Retrieve collection

Retrieves basic information about specified collection.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var getCollectionResult = db.Collection
    .Get("MyDocumentCollection");

if (getCollectionResult.Success)
{
    var id = getCollectionResult.Value.String("id");
    var name = getCollectionResult.Value.String("name");
    var isSystem = getCollectionResult.Value.Bool("isSystem");
    var status = getCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = getCollectionResult.Value.Enum<ArangoCollectionType>("type");
}
```

### Retrieve collection properties

Retrieves basic information with additional properties about specified collection.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var getCollectionResult = db.Collection
    .GetProperties("MyDocumentCollection");

if (getCollectionResult.Success)
{
    var id = getCollectionResult.Value.String("id");
    var name = getCollectionResult.Value.String("name");
    var waitForSync = getCollectionResult.Value.Bool("waitForSync");
    var isVolatile = getCollectionResult.Value.Bool("isVolatile");
    var isSystem = getCollectionResult.Value.Bool("isSystem");
    var status = getCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = getCollectionResult.Value.Enum<ArangoCollectionType>("type");
    var doCompact = getCollectionResult.Value.Bool("doCompact");
    var journalSize = getCollectionResult.Value.Long("journalSize");
    var keyGeneratorType = getCollectionResult.Value.Enum<ArangoKeyGeneratorType>("keyOptions.type");
    var allowUserKeys = getCollectionResult.Value.Bool("keyOptions.allowUserKeys");
}
```

### Retrieve collection count

Retrieves basic information with additional properties and document count in specified collection.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var getCollectionResult = db.Collection
    .GetCount("MyDocumentCollection");

if (getCollectionResult.Success)
{
    var id = getCollectionResult.Value.String("id");
    var name = getCollectionResult.Value.String("name");
    var waitForSync = getCollectionResult.Value.Bool("waitForSync");
    var isVolatile = getCollectionResult.Value.Bool("isVolatile");
    var isSystem = getCollectionResult.Value.Bool("isSystem");
    var status = getCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = getCollectionResult.Value.Enum<ArangoCollectionType>("type");
    var doCompact = getCollectionResult.Value.Bool("doCompact");
    var journalSize = getCollectionResult.Value.Long("journalSize");
    var keyGeneratorType = getCollectionResult.Value.Enum<ArangoKeyGeneratorType>("keyOptions.type");
    var allowUserKeys = getCollectionResult.Value.Bool("keyOptions.allowUserKeys");
    var count = getCollectionResult.Value.Long("count");
}
```

### Retrieve collection figures

Retrieves basic information with additional properties, document count and figures in specified collection.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var getCollectionResult = db.Collection
    .GetFigures("MyDocumentCollection");

if (getCollectionResult.Success)
{
    var id = getCollectionResult.Value.String("id");
    var name = getCollectionResult.Value.String("name");
    var waitForSync = getCollectionResult.Value.Bool("waitForSync");
    var isVolatile = getCollectionResult.Value.Bool("isVolatile");
    var isSystem = getCollectionResult.Value.Bool("isSystem");
    var status = getCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = getCollectionResult.Value.Enum<ArangoCollectionType>("type");
    var doCompact = getCollectionResult.Value.Bool("doCompact");
    var journalSize = getCollectionResult.Value.Long("journalSize");
    var keyGeneratorType = getCollectionResult.Value.Enum<ArangoKeyGeneratorType>("keyOptions.type");
    var allowUserKeys = getCollectionResult.Value.Bool("keyOptions.allowUserKeys");
    var count = getCollectionResult.Value.Long("count");
    var figures = getCollectionResult.Value.Document("figures");
}
```

### Retrieve collection revision

Retrieves basic information and revision ID of specified collection.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var getCollectionResult = db.Collection
    .GetRevision("MyDocumentCollection");

if (getCollectionResult.Success)
{
    var id = getCollectionResult.Value.String("id");
    var name = getCollectionResult.Value.String("name");
    var isSystem = getCollectionResult.Value.Bool("isSystem");
    var status = getCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = getCollectionResult.Value.Enum<ArangoCollectionType>("type");
    var revision = getCollectionResult.Value.String("revision");
}
```

### Retrieve collection checksum

Retrieves basic information, revision ID and checksum of specified collection.

Applicable optional parameters available through fluent API:

- `WithRevisions(bool value)` - Determines whether to include document revision ids in the checksum calculation. Default value: false.
- `WithData(bool value)` - Determines whether to include document body data in the checksum calculation. Default value: false.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var getCollectionResult = db.Collection
    .GetChecksum("MyDocumentCollection");

if (getCollectionResult.Success)
{
    var id = getCollectionResult.Value.String("id");
    var name = getCollectionResult.Value.String("name");
    var isSystem = getCollectionResult.Value.Bool("isSystem");
    var status = getCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = getCollectionResult.Value.Enum<ArangoCollectionType>("type");
    var revision = getCollectionResult.Value.String("revision");
    var checksum = getCollectionResult.Value.Long("checksum");
}
```

### Retrieve all documents

Retrieves list of documents in specified collection.

Applicable optional parameters available through fluent API:

- `ReturnListType(ArangoReturnListType value)` - Determines which attribute will be retuned in the list. Default value: Path.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var getDocumentsResult = db.Collection
    .ReturnListType(ArangoReturnListType.Key)
    .GetAllDocuments("MyDocumentCollection");

if (getDocumentsResult.Success)
{
    foreach (var item in getDocumentsResult.Value)
    {
        var key = item;
    }
}
```

### Retrieve all edges

Retrieves list of edges in specified collection.

Applicable optional parameters available through fluent API:

- `ReturnListType(ArangoReturnListType value)` - Determines which attribute will be retuned in the list. Default value: Path.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var getEdgesResult = db.Collection
    .ReturnListType(ArangoReturnListType.Key)
    .GetAllEdges("MyEdgeCollection");

if (getEdgesResult.Success)
{
    foreach (var item in getEdgesResult.Value)
    {
        var key = item;
    }
}
```

### Truncate collection

Removes all documents from specified collection.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var truncateCollectionResult = db.Collection
    .Truncate("MyDocumentCollection");

if (truncateCollectionResult.Success)
{
    var id = truncateCollectionResult.Value.String("id");
    var name = truncateCollectionResult.Value.String("name");
    var isSystem = truncateCollectionResult.Value.Bool("isSystem");
    var status = truncateCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = truncateCollectionResult.Value.Enum<ArangoCollectionType>("type");
}
```

### Load collection

Loads specified collection into memory.

Applicable optional parameters available through fluent API:

- `Count(bool value)` - Determines whether the return value should include the number of documents in collection. Default value: true.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var loadCollectionResult = db.Collection
    .Count(false)
    .Load("MyDocumentCollection");

if (loadCollectionResult.Success)
{
    var id = loadCollectionResult.Value.String("id");
    var name = loadCollectionResult.Value.String("name");
    var isSystem = loadCollectionResult.Value.Bool("isSystem");
    var status = loadCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = loadCollectionResult.Value.Enum<ArangoCollectionType>("type");
}
```

### Unload collection

Unloads specified collection from memory.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var unloadCollectionResult = db.Collection
    .Unload("MyDocumentCollection");

if (unloadCollectionResult.Success)
{
    var id = unloadCollectionResult.Value.String("id");
    var name = unloadCollectionResult.Value.String("name");
    var isSystem = unloadCollectionResult.Value.Bool("isSystem");
    var status = unloadCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = unloadCollectionResult.Value.Enum<ArangoCollectionType>("type");
}
```

### Change collection properties

Changes properties of specified collection.

Applicable optional parameters available through fluent API:

- `WaitForSync(bool value)` - Determines whether to wait until data are synchronised to disk. Default value: false.
- `JournalSize(long value)` - Determines maximum size of a journal or datafile in bytes. Default value: server configured.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var changeCollectionResult = db.Collection
    .WaitForSync(true)
    .JournalSize(1999999999)
    .ChangeProperties("MyDocumentCollection");

if (changeCollectionResult.Success)
{
    var id = changeCollectionResult.Value.String("id");
    var name = changeCollectionResult.Value.String("name");
    var waitForSync = changeCollectionResult.Value.Bool("waitForSync");
    var isVolatile = changeCollectionResult.Value.Bool("isVolatile");
    var isSystem = changeCollectionResult.Value.Bool("isSystem");
    var status = changeCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = changeCollectionResult.Value.Enum<ArangoCollectionType>("type");
    var doCompact = changeCollectionResult.Value.Bool("doCompact");
    var journalSize = changeCollectionResult.Value.Long("journalSize");
    var keyGeneratorType = changeCollectionResult.Value.Enum<ArangoKeyGeneratorType>("keyOptions.type");
    var allowUserKeys = changeCollectionResult.Value.Bool("keyOptions.allowUserKeys");
}
```

### Rename collection

Renames specified collection.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var renameCollectionResult = db.Collection
    .Rename("MyDocumentCollection", "MyFooCollection");

if (renameCollectionResult.Success)
{
    var id = renameCollectionResult.Value.String("id");
    var name = renameCollectionResult.Value.String("name");
    var isSystem = renameCollectionResult.Value.Bool("isSystem");
    var status = renameCollectionResult.Value.Enum<ArangoCollectionStatus>("status");
    var type = renameCollectionResult.Value.Enum<ArangoCollectionType>("type");
}
```

### Rotate collection journal

Rotates the journal of specified collection to make the data in the file available for compaction. Current journal of the collection will be closed and turned into read-only datafile. This operation is not available in cluster environment.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var rotateJournalResult = db.Collection
    .RotateJournal("MyDocumentCollection");
```

### Delete collection

Deletes specified collection.

```csharp
var db = new ArangoDatabase("myDatabaseAlias");

var deleteCollectionResult = db.Collection
    .Delete("MyDocumentCollection");

if (deleteCollectionResult.Success)
{
    var id = deleteCollectionResult.Value.String("id");
}
```