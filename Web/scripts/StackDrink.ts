/// <reference path="drink.ts" />

class StackDrink {
    drinks: KnockoutObservableArray<Drink>;
    setFromDTO: (drinks: DrinkModelDTO[]) => void;
    newDrink: KnockoutObservable<Drink>;
    showNewDrink: () => void;
    showDrink: (drink:Drink) => void;
    addDrinkToRepository: () => void;
    total: KnockoutObservable<number>;
    constructor(drinks: DrinkModelDTO[], stackCoin: KnockoutObservable<StackCoin>) {
        var self = this;

        self.drinks = ko.observableArray([]);
        self.setFromDTO = function (drinks: DrinkModelDTO[]) {
            self.drinks.removeAll();
            ko.utils.arrayMap(drinks, function (drink) { return self.drinks.push(new Drink(drink, stackCoin)) });
        }; self.setFromDTO(drinks);

        self.newDrink = ko.observable();
        self.showNewDrink = function () {
            self.newDrink(new Drink(new DrinkModelDTO(0, "", "", 0, 0, 0), stackCoin));
        }; self.showNewDrink();
        self.showDrink = function (drink) { self.newDrink(drink);};
        self.addDrinkToRepository = function () {
            $.post(`/dvm/api/drinks/`, self.newDrink().getToDTO())
            .done(function (drinks: DrinkModelDTO[]) {
                self.setFromDTO(drinks);
                self.showNewDrink();
            })
            .fail(function () { alert("Не удалось добавить напитки в хранилище."); });
        };
        self.total = ko.computed(function () {
            if (self.drinks().length === 0) return 0;
            var sum = 0;
            ko.utils.arrayForEach(self.drinks(), function (drink) { sum += drink.value(); });
            return sum;
        });
    };
};