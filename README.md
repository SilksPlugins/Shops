# Shops
A universal OpenMod plugin which adds Shop functionality.

## Commands
Commands for shop management:

- /shop add <buy | sell> <item> <price> - Adds the item to the shop to be bought or sold.
- /shop remove <buy | sell> <item> - Removes the buyable/sellable item from the shop.
- /vshop add <vehicle> <price> - Adds the vehicle to the shop to be bought.
- /vshop remove <vehicle> - Removes the buyable vehicle from the shop.
- /shop reload - Reloads the shops from the database.

User commands:
- /cost <item> [amount] - Checks the price of an item in the shop.
- /buy <item> [amount] - Buys the item from the shop.
- /sell <item> [amount] - Sells the item to the shop.
- /vcost <vehicle> - Checks the price of a vehicle in the shop.
- /vbuy <vehicle> - Buys the vehicle from the shop.

## Migration from v1.0.0

Due to some limitations, migration from v1.0.0 must be manual. The following MySQL query can be executed to migrate data from the old database tables to the new:
```sql
INSERT INTO Shops_ItemShops (ItemId, BuyPrice, SellPrice) (SELECT B.ID, BuyPrice, SellPrice FROM Shops_BuyItemShops B
LEFT OUTER JOIN Shops_SellItemShops S ON B.ID=S.ID
UNION
SELECT S.ID, BuyPrice, SellPrice FROM development.Shops_BuyItemShops B
RIGHT OUTER JOIN Shops_SellItemShops S ON B.ID=S.ID);

INSERT INTO Shops_VehicleShops (VehicleId, BuyPrice) SELECT ID, BuyPrice FROM Shops_BuyVehicleShops;
```
