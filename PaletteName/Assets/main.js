CharacterCreator.generatePalettesOld = CharacterCreator.generatePalettes;
CharacterCreator.generatePalettes = function () {
    CharacterCreator.generatePalettesOld();

    $('.Palette').each(function () {
        var key = $(this).data('name');
        var index = CharacterCreator.data[key].value;
        var paletteList = CharacterCreator.categoryData[key][index].palettes;

        for (var i = 0; i < paletteList.max; ++i) {
            var el = $(this).find('#Palette_' + i);
            if (el && paletteList[i].label) {

                el.data('label', paletteList[i].label);
            }
        }
    });

    $('.Palette_Colour_Holder').on('mouseover', function() {
        var label = $(this).data('label');
        if (typeof label !== 'undefined') {

            var s = label.split(".");
            var v = s.pop();
            $('#CharacterCreator_Info2Name').text(v);
            $('#CharacterCreator_Info2').removeClass('charactercreator-info2-name').addClass('charactercreator-info2-name');
        }
    });

    $('.Palette_Colour_Holder').on('mouseout', function () {
        $('#CharacterCreator_Info2Name').text('');
        $('#CharacterCreator_Info2').removeClass('charactercreator-info2-name')
    });
}