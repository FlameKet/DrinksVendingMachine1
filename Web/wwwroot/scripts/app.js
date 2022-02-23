/// <reference path="../node_modules/@types/knockout/index.d.ts" />
var Coin = /** @class */ (function () {
    function Coin(item) {
        var self = this;
        self.id = item.id;
        self.par = item.par;
        self.blocking = ko.observable(item.blocking);
        self.quantity = ko.observable(item.quantity);
        self.value = ko.computed(function () { return self.quantity() * item.par; });
        self.putOne = function () {
            if (!self.blocking())
                self.quantity(self.quantity() + 1);
            else
                alert("Монеты достоинством " + self.par + " не принимаются. Заберите ее из приемника.");
        };
        self.getToDTO = function () { return new CoinModelDTO(self.id, self.par, self.blocking(), self.quantity()); };
        self.coinAccessible = ko.computed(function () {
            return self.quantity() > 0;
        });
        self.quantityForAdding = ko.observable(0);
        self.plus = function () { self.save(self.quantityForAdding()); };
        self.minus = function () { self.save(-self.quantityForAdding()); };
        self.save = function (quantity) {
            $.ajax({
                url: "/dvm/api/coins/" + self.id + "/" + quantity,
                type: 'PUT',
                contentType: "application/json"
            })
                .done(function (data) {
                self.quantity(data.quantity);
                self.quantityForAdding(0);
            })
                .fail(function () {
                if (quantity > 0)
                    alert("Не удалось добавить монеты в хранилище.");
                else
                    alert("Не удалось забрать монеты из хранилища.");
            });
        };
        self.changeBlocking = function () {
            $.ajax({
                url: "/dvm/api/coins/" + self.id + "/blocking/" + !self.blocking(),
                type: 'PUT',
                contentType: "application/json"
            })
                .done(function (data) { self.blocking(data.blocking); })
                .fail(function () { alert("Не удалось заблокировать/разблокировать монету."); });
        };
        self.btnClass = ko.computed(function () {
            if (self.quantityForAdding() > 0)
                return "btn btn-primary btn-sm";
            return "btn btn-primary btn-sm disabled";
        });
        self.btnBlockingClass = ko.computed(function () {
            if (self.blocking())
                return "btn btn-primary btn-sm";
            return "btn btn-primary btn-sm disabled";
        });
    }
    return Coin;
}());
var CoinModelDTO = /** @class */ (function () {
    function CoinModelDTO(id, par, blocking, quantity) {
        this.id = id;
        this.par = par;
        this.blocking = blocking;
        this.quantity = quantity;
    }
    return CoinModelDTO;
}());
;
/// <reference path="../node_modules/@types/knockout/index.d.ts" />
var Drink = /** @class */ (function () {
    function Drink(item, stackCoin) {
        var self = this;
        self.id = item.id;
        self.name = item.name;
        self.image = item.image;
        self.volume = item.volume;
        self.price = ko.observable(item.price);
        self.oldPrice = ko.observable(item.price);
        self.quantity = ko.observable(item.quantity);
        self.value = ko.computed(function () { return self.quantity() * self.price(); });
        self.drinkAccessible = ko.computed(function () { return self.quantity() > 0; });
        self.canBeSold = ko.computed(function () {
            return self.drinkAccessible() && stackCoin() && stackCoin().total() >= self.price();
        });
        self.sell = function () {
            if (self.canBeSold()) {
                $.ajax({
                    url: '/dvm/api/coins/putDepositReturnBalance/' + (stackCoin().total() - self.price()),
                    data: JSON.stringify(stackCoin().getToDTO()),
                    type: 'POST',
                    dataType: "json",
                    contentType: "application/json"
                })
                    .done(function (returnedDeposit) {
                    stackCoin().setFromDTO(returnedDeposit);
                    $.ajax({
                        url: "/dvm/api/drinks/" + self.id + "/giveOut",
                        type: 'PUT',
                        contentType: "application/json"
                    })
                        .done(function (data) {
                        self.quantity(data.quantity);
                        alert("Заберите напиток");
                    })
                        .fail(function () { alert("Не удалось выдать напиток."); });
                })
                    .fail(function (err) {
                    alert("Ошибка. " + err.responseText);
                });
            }
        };
        self.getToDTO = function () { return new DrinkModelDTO(self.id, self.name, self.image, self.volume, self.price(), self.quantity()); };
        self.priceNeedSave = ko.computed(function () { return self.price() !== self.oldPrice(); });
        self.btnClass = ko.computed(function () {
            if (self.priceNeedSave())
                return "btn btn-primary btn-sm";
            return "btn btn-primary btn-sm disabled";
        });
        self.priceSave = function () {
            $.ajax({
                url: "/dvm/api/drinks/" + self.id + "/price/" + self.price(),
                type: 'PUT',
                contentType: "application/json"
            })
                .done(function (data) {
                self.price(data.price);
                self.oldPrice(data.price);
            })
                .fail(function () {
                alert("Не удалось изменить цену.");
                self.price(self.oldPrice());
            });
        };
        self.remove = function () {
            $.ajax({
                url: "/dvm/api/drinks/" + self.id,
                type: 'DELETE',
                contentType: "application/json"
            })
                .done(function () { self.quantity(0); })
                .fail(function () { alert("Не удалось удалить напитки."); });
        };
    }
    return Drink;
}());
var DrinkModelDTO = /** @class */ (function () {
    function DrinkModelDTO(id, name, image, volume, price, quantity) {
        this.id = id;
        this.name = name;
        this.image = image;
        this.volume = volume;
        this.price = price;
        this.quantity = quantity;
    }
    return DrinkModelDTO;
}());
;
/// <reference path="coin.ts" />
var StackCoin = /** @class */ (function () {
    function StackCoin(coins) {
        var self = this;
        self.coins = ko.observableArray();
        self.clearCoins = function () { ko.utils.arrayForEach(self.coins(), function (coin) { coin.quantity(0); }); };
        self.refund = function () {
            self.clearCoins();
            alert("Заберите монеты.");
        };
        self.setFromDTO = function (coins) {
            self.coins.removeAll();
            ko.utils.arrayMap(coins, function (coin) { return self.coins.push(new Coin(coin)); });
        };
        self.setFromDTO(coins);
        self.getToDTO = ko.computed(function () {
            if (!self.coins())
                return undefined;
            return ko.utils.arrayMap(self.coins(), function (coin) { return coin.getToDTO(); });
        });
        self.total = ko.computed(function () {
            if (!self.coins())
                return 0;
            if (self.coins().length === 0)
                return 0;
            var sum = 0;
            ko.utils.arrayForEach(self.coins(), function (coin) { sum += coin.value(); });
            return sum;
        });
    }
    return StackCoin;
}());
/// <reference path="drink.ts" />
var StackDrink = /** @class */ (function () {
    function StackDrink(drinks, stackCoin) {
        var self = this;
        self.drinks = ko.observableArray([]);
        self.setFromDTO = function (drinks) {
            self.drinks.removeAll();
            ko.utils.arrayMap(drinks, function (drink) { return self.drinks.push(new Drink(drink, stackCoin)); });
        };
        self.setFromDTO(drinks);
        self.newDrink = ko.observable();
        self.showNewDrink = function () {
            self.newDrink(new Drink(new DrinkModelDTO(0, "", "", 0, 0, 0), stackCoin));
        };
        self.showNewDrink();
        self.showDrink = function (drink) { self.newDrink(drink); };
        self.addDrinkToRepository = function () {
            $.post("/dvm/api/drinks/", self.newDrink().getToDTO())
                .done(function (drinks) {
                self.setFromDTO(drinks);
                self.showNewDrink();
            })
                .fail(function () { alert("Не удалось добавить напитки в хранилище."); });
        };
        self.total = ko.computed(function () {
            if (self.drinks().length === 0)
                return 0;
            var sum = 0;
            ko.utils.arrayForEach(self.drinks(), function (drink) { sum += drink.value(); });
            return sum;
        });
    }
    ;
    return StackDrink;
}());
;
/// <reference path="StackDrink.ts" />
/// <reference path="StackCoin.ts" />
var Model = /** @class */ (function () {
    function Model() {
        var self = this;
        self.activeCoin = ko.observable(true);
        self.stackCoin = ko.observable(undefined);
        self.loadCoins = function (withСleaning) {
            $.getJSON('/dvm/api/coins')
                .done(function (data) {
                self.stackCoin(new StackCoin(data));
                if (withСleaning)
                    self.stackCoin().clearCoins();
                self.activeCoin(false);
            })
                .fail(function () {
                alert('Не удалось получить список доступных монет.');
                self.activeCoin(false);
            });
        };
        self.activeDrink = ko.observable(true);
        self.stackDrink = ko.observable(undefined);
        self.loadDrinks = function () {
            $.getJSON('/dvm/api/drinks')
                .done(function (data) {
                self.stackDrink(new StackDrink(data, self.stackCoin));
                self.activeDrink(false);
            })
                .fail(function () {
                alert('Не удалось получить список доступных напитков.');
                self.activeDrink(false);
            });
        };
    }
    ;
    return Model;
}());
;
//# sourceMappingURL=app.js.map