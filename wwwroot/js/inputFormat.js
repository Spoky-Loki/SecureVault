var cc = forma.CcNumber;
var cvv = forma.CVV;
var ccE = forma.CcExpiration;
var ccN = forma.CcName;
events = ['input', 'change', 'blur', 'keyup'];

for (var i in events) {
    cc.addEventListener(events[i], formatCardNumber, false);
    cvv.addEventListener(events[i], formatCardCVV, false);
    ccE.addEventListener(events[i], formatCardExpiration, false);
    ccN.addEventListener(events[i], formatCardName, false);
}

function formatCardNumber() {
    var cardCode = this.value.replace(/[^\d]/g, '').substring(0, 16);
    cardCode = cardCode != '' ? cardCode.match(/.{1,4}/g).join(' ') : '';
    this.value = cardCode;
}

function formatCardCVV() {
    var cardCVV = this.value.replace(/[^\d]/g, '').substring(0, 3);
    this.value = cardCVV;
}

function formatCardExpiration() {
    var cardExpiration = this.value.replace(/[^\d]/g, '').substring(0, 5);
    cardExpiration = cardExpiration != '' ? cardExpiration.match(/.{1,2}/g).join('/') : '';
    this.value = cardExpiration;
}

function formatCardName() {
    var cardName = this.value.replace(/[^a-zA-Zа-яА-Я ]/g, '').toUpperCase();
    this.value = cardName;
}
