# 📌 What is OData?

OData (Open Data Protocol) is a protocol designed to simplify data sharing and consumption in RESTful APIs. Developed by Microsoft, it enables flexible and powerful querying capabilities, making it easier to work with large datasets by providing built-in filtering, sorting, and pagination mechanisms.

# 🚀 Using OData with .NET 9

Integrating OData with .NET 9 requires just three essential steps:

## 1️⃣ **Install Microsoft.AspNetCore.OData Framework**

First, add the `Microsoft.AspNetCore.OData` package to your project:

![image](https://github.com/user-attachments/assets/64a9eb73-2d39-4b33-970e-3a5683713d25)


## 2️⃣ **Enable OData in Program.cs**

Modify `Program.cs` by adding OData to the Controllers section and calling the `EnableQueryFeatures` function.

```csharp
builder.Services.AddControllers().AddOData(opt =>
        opt.EnableQueryFeatures()
);
```

## 3️⃣ **Enable OData in the Controller**

Within your Controller, add the `[EnableQuery]` annotation and call the data using `IQueryable`:

```csharp
[Route("api/[controller]")]
[ApiController]
public sealed class TestController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("Categories")]
    [EnableQuery]
    public IQueryable<Category> Categories()
    {
        var categories = context.Categories.AsQueryable();
        return categories;
    }
}
```

Once these steps are completed, you can include filtering parameters in API requests via the **Query Parameters** section. The use of `IQueryable` in the Controller ensures that filtering is applied dynamically. This is achieved by invoking `app.MapControllers()` in **Program.cs** before `app.Run()`, enabling seamless integration of filter parameters into the query execution process.

![image](https://github.com/user-attachments/assets/bfd3a109-f833-41ff-8c95-e652291bbaab)

## **Using OData Query Parameters**

OData supports various query parameters for filtering and sorting data:

- `$top` → Specifies the number of records to retrieve.
- `$skip` → Skips a defined number of records.
- `$orderby` → Sorts the data by a given field.
- `$filter` → Filters data based on specific criteria.
- `$select` → Selects specific fields.
- `$expand` → Includes related entities in the result set.

![image](https://github.com/user-attachments/assets/ad8ec684-b85b-4684-be42-ee8960ad5acd)

### Example request:
```
GET /api/Categories?$top=10&$orderby=Name asc
```

# 🔒 Restricting OData Query Options

You can control which query options are allowed in OData requests.

**Allow only `$top` and `$select` options:**

```csharp
[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Top | AllowedQueryOptions.Select)]
```

**Disable specific query options:**

```csharp
[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All & ~AllowedQueryOptions.Select)]
```

# 🛠 Restricting OData in Program.cs

To apply global OData restrictions, modify `Program.cs` as follows:

```csharp
builder.Services.AddControllers().AddOData(opt =>
        opt
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
);
```

# 📊 Implementing OData Pagination and EDM Models

Pagination is crucial for handling large datasets efficiently. OData allows defining an **Entity Data Model (EDM)** to manage structured queries and metadata. To implement this, modifications are required in both `Controller.cs` and `Program.cs`.
⚠️ ATTENTION: Here, ODataController must be used instead of ControllerBase.

## **TestController.cs**

```csharp
[Route("odata")]
[ApiController]
public sealed class TestController(ApplicationDbContext context) : ODataController
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EntitySet<Category>("Categories");
        return builder.GetEdmModel();
    }

    [HttpGet("Categories")]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All & ~AllowedQueryOptions.Select)]
    public IQueryable<Category> Categories()
    {
        var categories = context.Categories.AsQueryable();
        return categories;
    }
}
```

## **Adding Route Configuration in Program.cs**

```csharp
builder.Services.AddControllers().AddOData(opt =>
        opt
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
        .AddRouteComponents("odata", TestController.GetEdmModel())
);
```

With this setup, the OData API will now provide an `@odata.context` field in the response, containing metadata about the queried entities and fields.

Additionally, the API response will include a `value` list displaying the retrieved records. By adding `$count=true` to the query parameters, the total record count can be accessed, which is particularly useful for pagination implementations.

![image](https://github.com/user-attachments/assets/bbcedced-9a8a-4257-9698-eba3e0520d7e)

---

📌 **Key Takeaways:**

- OData provides a flexible querying mechanism for RESTful APIs.
- It enhances efficiency when working with large datasets.
- Supports advanced filtering, sorting, and pagination.

🚀 For further details, refer to [Microsoft's official OData documentation](https://www.odata.org/)!

---

## 📌 OData Nedir?

OData (Open Data Protocol), RESTful API'lerin veri paylaşımını ve tüketimini kolaylaştıran bir protokoldür. Microsoft tarafından geliştirilen bu protokol, API'lerin esnek ve güçlü sorgulamalar yapmasına olanak tanır. OData, özellikle büyük veri kümeleriyle çalışırken filtreleme, sıralama ve sayfalama gibi işlemleri kolaylaştırır.

## 🚀 OData'yı .NET 9 ile Kullanma

.NET 9 ile OData kullanmak için sadece üç temel adım gereklidir:

### 1️⃣ **Microsoft.AspNetCore.OData Framework'ünü Ekleyin**

Öncelikle, `Microsoft.AspNetCore.OData` paketini projenize eklemelisiniz:

![image](https://github.com/user-attachments/assets/64a9eb73-2d39-4b33-970e-3a5683713d25)

### 2️⃣ OData’yı Ekliyoruz

Daha sonrasında Program.cs dosyamız içerisinde Controllers kısmının sonuna OData’yı ekliyoruz ve EnableQueryFeatures fonksiyonunu çağırıyoruz.

```csharp
builder.Services.AddControllers().AddOData(opt =>
        opt.EnableQueryFeatures()
);
```

### 3️⃣**Controller İçinde OData’yı Aktifleştirin**

Controller’da `EnableQuery` özelliğini ekleyerek veriyi `IQueryable` türünde çağırabilirsiniz:

```csharp
[Route("api/[controller]")]
[ApiController]
public sealed class TestController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("Categories")]
    [EnableQuery]
    public IQueryable<Category> Categories()
    {
        var categories = context.Categories.AsQueryable();
        return categories;
    }
}
```

Bu işlemler tamamlandıktan sonra, API'ye gönderilen isteklerde **Query Parameters** bölümüne gerekli filtreleme kriterlerini ekleyebiliriz. Controller içinde veriyi **Queryable** olarak çağırmamız, **Program.cs** dosyasında `app.Run()` komutundan önce `app.MapControllers()` metodunun çağrıldığını gösterir. Bu yapı sayesinde, **Queryable** olarak alınan filtreleme parametreleri **Query** nesnesine entegre edilerek işlenir ve ardından yürütülür.

![image](https://github.com/user-attachments/assets/bfd3a109-f833-41ff-8c95-e652291bbaab)


### **OData Query Parametrelerini Kullanma**

API’ye gönderilen isteklerde OData’nın sunduğu `Query Parameters` ile çeşitli filtreleme ve sıralama işlemleri yapabilirsiniz:

- `$top` → Veritabanından kaç kayıt getirileceğini belirler.
- `$skip` → Kaç kaydın atlanacağını belirler.
- `$orderby` → Verileri sıralamak için kullanılır.
- `$filter` → Belirli bir kritere göre filtreleme yapar.
- `$select` → Belirli alanları getirir.
- `$expand` → `Include` işlemi yaparak ilişkili verileri getirir.

![image](https://github.com/user-attachments/assets/bfd3a109-f833-41ff-8c95-e652291bbaab)

Örnek istek:

```
GET /api/Categories?$top=10&$orderby=Name asc
```

## 🔒 OData Kısıtlama Seçenekleri

OData sorgularını belirli filtrelerle sınırlandırabilirsiniz.

**Sadece `$top` ve `$select` kullanabilmesini sağlamak için:**

```csharp
[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Top | AllowedQueryOptions.Select)]
```

**Belirli sorgu seçeneklerini devre dışı bırakmak için:**

```csharp
[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All & ~AllowedQueryOptions.Select)]

```

## 🛠 Program.cs Üzerinde Kısıtlama Yapma

OData’da genel bir kısıtlama tanımlamak için `Program.cs` dosyanıza şu kodu ekleyebilirsiniz:

```csharp
builder.Services.AddControllers().AddOData(opt =>
        opt
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
);
```

## 📊 OData Pagination ve EDM Model Kullanımı

Pagination yapısında tüm verilerin sayısını görebilmek önemlidir. OData ile `IEdmModel` kullanarak model tanımlayabilirsiniz. Bu yapı için `Controller.cs`  ve  `Program.cs`  dosyalarında değişiklikler yapmalısınız.
⚠️ DİKKAT: Burada ControllerBase yerine ODataController kullanılması gerekmektedir.

### **TestController.cs**

```csharp
[Route("odata")]
[ApiController]
public sealed class TestController(ApplicationDbContext context) : ODataController
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EntitySet<Category>("Categories");
        return builder.GetEdmModel();
    }

    [HttpGet("Categories")]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All & ~AllowedQueryOptions.Select)]
    public IQueryable<Category> Categories()
    {
        var categories = context.Categories.AsQueryable();
        return categories;
    }
}

```

### **Program.cs İçinde Route Tanımlaması**

```csharp
builder.Services.AddControllers().AddOData(opt =>
        opt
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
        .AddRouteComponents("odata", TestController.GetEdmModel())
);
```

Bu yapı ile OData API’niz artık bir `@odata.context` sağlayacak ve bağlandığınız tabloların şemasını içeren bir obje olarak dönecektir.

Ayrıca, API'den dönen JSON nesnesi içerisinde `value` listesi altında verileri görebilirsiniz. Ek olarak, `params` listesinde `$count=true` parametresini gönderdiğinizde, tüm veri sayısına erişebilirsiniz. Bu, özellikle pagination uygulamaları için oldukça önemlidir.

![image](https://github.com/user-attachments/assets/bbcedced-9a8a-4257-9698-eba3e0520d7e)

---

📌 **Önemli Not:**

- OData, RESTful API'ler için esnek bir sorgulama mekanizması sağlar.
- Büyük veri kümeleriyle çalışırken verimlilik kazandırır.
- İleri seviye filtreleme ve pagination seçenekleri sunar.

✅ **Geliştirme Önerileri**:

1. **Daha fazla kod örneği ekleyerek kullanım senaryolarını genişletebiliriz.**
2. **Görseller ve diyagramlar ile OData'nın çalışma prensibini anlatabiliriz.**
3. **Özellikle pagination ve include işlemlerinin performansa etkisini analiz edebiliriz.**

🚀 Daha fazla bilgi için [Microsoft’un OData dokümantasyonunu](https://www.odata.org/) inceleyebilirsiniz!
