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

    },

    disableArrowScroll: function () {
        document.addEventListener('keydown', function(event) {
            if (["ArrowLeft", "ArrowUp", "ArrowRight", "ArrowDown"].indexOf(event.code) > -1) {
                event.preventDefault();
            }
        }, false) 
    },
    
};