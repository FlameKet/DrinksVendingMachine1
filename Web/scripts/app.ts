/// <reference path="StackDrink.ts" />
/// <reference path="StackCoin.ts" />

class Model {
    activeCoin: KnockoutObservable<boolean>;
    loadCoins: (withСleaning: boolean) => void;
    stackCoin: KnockoutObservable<StackCoin>;

    activeDrink: KnockoutObservable<boolean>;
    loadDrinks: () => void;
    stackDrink: KnockoutObservable<StackDrink>;
    constructor() {
        var self = this;

        self.activeCoin = ko.observable(true);
        self.stackCoin = ko.observable(undefined);
        self.loadCoins = function (withСleaning: boolean) {
            $.getJSON('/dvm/api/coins')
                .done(function (data: CoinModelDTO[]) {
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
                .done(function (data: DrinkModelDTO[]) {
                    self.stackDrink(new StackDrink(data, self.stackCoin))
                    self.activeDrink(false);
                })
                .fail(function () {
                    alert('Не удалось получить список доступных напитков.');
                    self.activeDrink(false);
                });
        };
    };
};