/// <reference path="../node_modules/@types/knockout/index.d.ts" />

class Coin {
    id: number;
    par: number;
    blocking: KnockoutObservable<boolean>;
    quantity: KnockoutObservable<number>
    clear: () => void;
    value: KnockoutComputed<number>
    putOne: () => void;
    getToDTO: () => CoinModelDTO;
    coinAccessible: KnockoutComputed<boolean>
    quantityForAdding: KnockoutObservable<number>
    plus: () => void;
    minus: () => void;
    save: (quantity: number) => void;
    changeBlocking: () => void;
    btnClass: KnockoutObservable<string>
    btnBlockingClass: KnockoutObservable<string>

    constructor(item: CoinModelDTO) {
        var self = this;
        self.id = item.id;
        self.par = item.par;
        self.blocking = ko.observable(item.blocking);
        self.quantity = ko.observable(item.quantity);
        self.value = ko.computed(function () { return self.quantity() * item.par; });
        self.putOne = function () {
            if (!self.blocking()) self.quantity(self.quantity() + 1);
            else alert("Монеты достоинством " + self.par + " не принимаются. Заберите ее из приемника.");
        }
        self.getToDTO = function () { return new CoinModelDTO(self.id, self.par, self.blocking(), self.quantity()) }
        self.coinAccessible = ko.computed(function () {
            return self.quantity() > 0;
        });
        self.quantityForAdding = ko.observable(0);
        self.plus = function () { self.save(self.quantityForAdding()); };
        self.minus = function () { self.save(-self.quantityForAdding()); };
        self.save = function (quantity) {
            $.ajax({
                url: `/dvm/api/coins/${self.id}/${quantity}`,
                type: 'PUT',
                contentType: "application/json"
            })
                .done(function (data:CoinModelDTO) {
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
                url: `/dvm/api/coins/${self.id}/blocking/${!self.blocking()}`,
                type: 'PUT',
                contentType: "application/json"
            })
                .done(function (data:CoinModelDTO) { self.blocking(data.blocking); })
                .fail(function () { alert("Не удалось заблокировать/разблокировать монету."); });
        };
        self.btnClass = ko.computed(function () {
            if (self.quantityForAdding() > 0) return "btn btn-primary btn-sm";
            return "btn btn-primary btn-sm disabled";
        });
        self.btnBlockingClass = ko.computed(function () {
            if (self.blocking() )
                return "btn btn-primary btn-sm";
            return "btn btn-primary btn-sm disabled";
        });
    }
}

class CoinModelDTO {
    id: number;
    par: number;
    blocking: boolean;
    quantity: number;
    constructor(id: number, par: number, blocking: boolean, quantity: number) {
        this.id = id;
        this.par = par;
        this.blocking = blocking;
        this.quantity = quantity;
    }
};