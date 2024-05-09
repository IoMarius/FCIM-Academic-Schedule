function _romanianLetterCheck(value) {
     var re = /^[a-zA-ZăâîșțĂÂÎȘȚ]+$/;
     return !re.test(value);
}

function _checkForDigits(text) {
	var re = /[0-9]/;
	return !re.test(text);
}

function _emailCheck(value) {
     var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/i;
     return !re.test(value);
}
function _equalToCheck(value, targetValue) {
	return (value.trim() !== targetValue.trim()) ? true : false;
}

/* Min value validation */
function _minValueCheck(value, minVal) {
	var val = value.trim();

	if (_numberCheck(val)) {
		return true;
	}
	return (val < minVal) ? true : false;
}

/* Max value validation */
function _maxValueCheck(value, maxVal) {
	var val = value.trim();

	if (_numberCheck(val)) {
		return true;
	}
	return (val > maxVal) ? true : false;
}

/* Range value validation */
function _rangeValueCheck(value, rangeVal) {
	var val = value.trim();

	if (_numberCheck(val)) {
		return true;
	}
	return (val < rangeVal[0] || val > rangeVal[1]) ? true : false;
}

/* Min length validation */
function _minLengthCheck(value, minLen) {
	return (value.trim().length < minLen) ? true : false;
}

/* Max length validation */
function _maxLengthCheck(value, maxLen) {
	return (value.trim().length > maxLen) ? true : false;
}

/* Range length validation */
function _rangeLengthCheck(value, rangeLen) {
	var val = value.trim().length;
	return (val < rangeLen[0] || val > rangeLen[1]) ? true : false;
}

/* Integer validation */
function _integerCheck(value) {
	var re = /^-?\d+$/;
	return !re.test(value);
}

/* Number validation */
function _numberCheck(value) {
	var re = /^-?\d+(?:\.\d+)?$/;
	return !re.test(value);
}




