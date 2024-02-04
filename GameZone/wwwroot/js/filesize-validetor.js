$.validator.addMethod('filesize', function (value, element, paramter) {
    return this.optional(element) || element.files[0].size <= paramter;
});