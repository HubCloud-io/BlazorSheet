window.blazorSheet = {

    focusElement: function (id) {
        const element = document.getElementById(id);

        if (element != null) {
            element.focus();
        }
    },

    getElementCoordinates : function (id)  {

        const element = document.getElementById(id);

        if(element != null){
            const rect = element.getBoundingClientRect();
            return {
                top: rect.top + window.scrollY,
                left: rect.left + window.scrollX,
                width: rect.width,
                height: rect.height
            };
        }

    }
    
};