Data sample: https://www.kaggle.com/datafiniti/womens-shoes-prices/download

Elastic search library: https://bytefish.de/blog/elasticsearch_net/

Redis setup: https://www.codeproject.com/Articles/636730/Distributed-Caching-using-Redis

CREATE TABLE Product
(
	ProductId int identity(1,1) PRIMARY KEY,
	ProductName nvarchar(500),
	ProductDescription nvarchar(MAX),
	ProductBrand nvarchar(500),
	ProductCategory nvarchar(500),
	ProductType nvarchar(500),
	ProductPrice decimal(18,0),
	ProductImage nvarchar(MAX)
);

