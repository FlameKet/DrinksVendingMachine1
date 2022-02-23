/// <reference path="coin.ts" />

class StackCoin {
    coins: KnockoutObservableArray<Coin>;
    total: KnockoutComputed<number>
    getToDTO: KnockoutComputed<CoinModelDTO[]>
    setFromDTO: (value: CoinModelDTO[]) => void;
    clearCoins: () => void;
    refund: () => void;

    constructor(coins: CoinModelDTO[]) {
        var self = this;
        self.coins = ko.observableArray();

        self.clearCoins = function () { ko.utils.arrayForEach(self.coins(), function (coin) { coin.quantity(0); }); }
        self.refund = function () {
            self.clearCoins();
            alert("Заберите монеты.");
        };
        self.setFromDTO = function (coins: CoinModelDTO[]) {
            self.coins.removeAll();
            ko.utils.arrayMap(coins, function (coin) { return self.coins.push(new Coin(coin)) });
        }; self.setFromDTO(coins);

        self.getToDTO = ko.computed(function () {
            if (!self.coins()) return undefined;
            return ko.utils.arrayMap(self.coins(), function (coin) { return coin.getToDTO();});
        });

        self.total = ko.computed(function () {
            if (!self.coins()) return 0;
            if (self.coins().length === 0) return 0;
            var sum = 0;
            ko.utils.arrayForEach(self.coins(), function (coin) { sum += coin.value(); });
            return sum;
        })
    }
}