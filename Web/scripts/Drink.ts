/// <reference path="../node_modules/@types/knockout/index.d.ts" />

class Drink{
    id: number;
    name: string;
    image: string;
    volume: number;
    price: KnockoutObservable<number>
    oldPrice: KnockoutObservable<number>
    quantity: KnockoutObservable<number>
    value: KnockoutObservable<number>
    drinkAccessible: KnockoutComputed<boolean>
    canBeSold: KnockoutComputed<boolean>
    canReturnChange: () => boolean;
    sell: () => void;
    getToDTO: () => DrinkModelDTO;
    priceNeedSave: KnockoutComputed<boolean>;
    btnClass: KnockoutComputed<string>;
    priceSave: () => void;
    remove: () => void;

    constructor(item: DrinkModelDTO, stackCoin: KnockoutObservable<StackCoin>) {
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
            return self.drinkAccessible() && stackCoin() && stackCoin().total() >= self.price()
        });
        self.sell = function () {
            if (self.canBeSold()) {
                $.ajax({
                    url:'/dvm/api/coins/putDepositReturnBalance/' + (stackCoin().total() - self.price()),
                    data: JSON.stringify(stackCoin().getToDTO()),
                    type: 'POST',
                    dataType: "json",
                    contentType: "application/json"
                })
                    .done(function (returnedDeposit: CoinModelDTO[]) {
                        stackCoin().setFromDTO(returnedDeposit);
                        $.ajax({
                            url: `/dvm/api/drinks/${self.id}/giveOut`,
                            type: 'PUT',
                            contentType: "application/json"
                        })
                            .done(function (data: DrinkModelDTO) {
                                self.quantity(data.quantity);
                                alert("Заберите напиток");
                            })
                            .fail(function () { alert("Не удалось выдать напиток."); });
                    })
                    .fail(function (err) {
                        alert("Ошибка. " + err.responseText);
                    });
            }
        }
        self.getToDTO = function () { return new DrinkModelDTO(self.id, self.name, self.image, self.volume, self.price(), self.quantity()) }

        self.priceNeedSave = ko.computed(function () { return self.price() !== self.oldPrice(); });
        self.btnClass = ko.computed(function () {
            if (self.priceNeedSave()) return "btn btn-primary btn-sm";
            return "btn btn-primary btn-sm disabled";
        });
        self.priceSave = function () {
            $.ajax({
                url: `/dvm/api/drinks/${self.id}/price/${self.price()}`,
                type: 'PUT',
                contentType: "application/json"
            })
                .done(function (data: DrinkModelDTO) {
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
                url: `/dvm/api/drinks/${self.id}`,
                type: 'DELETE',
                contentType: "application/json"
            })
                .done(function () { self.quantity(0); })
                .fail(function () { alert("Не удалось удалить напитки."); });
        };
    }
}

class DrinkModelDTO {
    id: number;
    name: string;
    image: string;
    volume: number;
    price: number;
    quantity: number;
    constructor(id: number, name: string, image: string, volume: number, price: number, quantity: number) {
        this.id = id;
        this.name = name;
        this.image = image;
        this.volume = volume;
        this.price = price;
        this.quantity = quantity;
    }
};