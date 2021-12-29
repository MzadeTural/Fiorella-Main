$(document).ready(function () {
    $(document).on('click', '#more_btn', function () {
        let prdCount = $(".products").children().length;

        let DbPrdCount = $("#ProducCount").val();
        //let apiData = await fetch("http://localhost:64749/Product/LoadProduct?skip=" + prdCount);
        //let datas = await apiData.text();

        //$(".products").append(datas);
        //prdCount = $(".products").children().length;
        //console.log("Prd:" + prdCount);
        //if (prdCount == DbPrdCount) {

        //    $("#more_btn").remove();
        //}
////////////////////////////////
        const url = "http://localhost:7104/product/LoadProduct";
        // Note: Depending on your API this may need to be different
        let data = new URLSearchParams();
        data.append(`skip`, prdCount);
        const options = {
            method: `POST`,
            body: data
        };
        fetch(url, options)
            .then(response =>response.text()) 
            .then(data => {
                $(".products").append(data);
                let prdCount = $(".products").children().length;
                if (prdCount == DbPrdCount) {
                    $("#more_btn").remove();
                }
            })

    })

    $(document).on('click', '#AddBasket', async function () {
      
        let DbPrdCount = $("#AddBasket").val();
        //$("#Basket").html(''); 
        //let apiData = await fetch("http://localhost:7104/product/AddBasket");
        //let datas = await apiData.text();
       // let itemId = prntli.getAttribute("asp-route-id");
        //$("#Basket").append(datas);
        const url = "http://localhost:7104/product/AddBasket";
        // Note: Depending on your API this may need to be different
        let data = new URLSearchParams();
        data.append(`id`, DbPrdCount);
        const options = {
            method: `POST`,
            body: data
        };
        fetch(url,options)
            .then(response => response.text())
            .then(data => {
                $("#Basket").append(data);
               
            })
    })
        $(document).ready(function () {
            //var apiBaseUrl = "http://localhost:7104/product/AddBasket";
            //$('#AddBasket').click(function () {
            //    $.ajax({
            //        url: apiBaseUrl,
            //        type: 'GET',
            //        dataType: 'json',
            //        success: function (data) {
            //            var $table = $('<table/>').addClass('dataTable table table-bordered table-striped');
            //            var $header = $('<thead/>').html('<tr><th>Name</th><th>Position</th><th>Office</th><th>Age</th><th>Salary</th></tr>');
            //            $table.append($header);
            //            $.each(data, function (i, val) {
            //                var $row = $('<tr/>');
            //                $row.append($('<td/>').html(val.Title));
            //                $row.append($('<td/>').html(val.Position));
            //                $row.append($('<td/>').html(val.Office));
            //                $row.append($('<td/>').html(val.Age));
            //                $row.append($('<td/>').html(val.Salary));
            //                $table.append($row);
            //            });
            //            $('#Basket').html($table);
            //        },
            //        error: function () {
            //            alert('Error!');
            //        }
            //    });
            //});
        });  
  



    // HEADER

    $(document).on('click', '#search',  function () {
        $(this).next().toggle();
    })

    $(document).on('click', '#mobile-navbar-close', function () {
        $(this).parent().removeClass("active");

    })
    $(document).on('click', '#mobile-navbar-show', function () {
        $('.mobile-navbar').addClass("active");

    })

    $(document).on('click', '.mobile-navbar ul li a', function () {
        if ($(this).children('i').hasClass('fa-caret-right')) {
            $(this).children('i').removeClass('fa-caret-right').addClass('fa-sort-down')
        }
        else {
            $(this).children('i').removeClass('fa-sort-down').addClass('fa-caret-right')
        }
        $(this).parent().next().slideToggle();
    })

    // SLIDER

    $(document).ready(function(){
        $(".slider").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });

    // PRODUCT

    $(document).on('click', '.categories', function(e)
    {
        e.preventDefault();
        $(this).next().next().slideToggle();
    })

    $(document).on('click', '.category li a', function (e) {
        e.preventDefault();
        let category = $(this).attr('data-id');
        let products = $('.product-item');
        
        products.each(function () {
            if(category == $(this).attr('data-id'))
            {
                $(this).parent().fadeIn();
            }
            else
            {
                $(this).parent().hide();
            }
        })
        if(category == 'all')
        {
            products.parent().fadeIn();
        }
    })

    // ACCORDION 

    $(document).on('click', '.question', function()
    {   
       $(this).siblings('.question').children('i').removeClass('fa-minus').addClass('fa-plus');
       $(this).siblings('.answer').not($(this).next()).slideUp();
       $(this).children('i').toggleClass('fa-plus').toggleClass('fa-minus');
       $(this).next().slideToggle();
       $(this).siblings('.active').removeClass('active');
       $(this).toggleClass('active');
    })

    // TAB

    $(document).on('click', 'ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().next().children('p.active').removeClass('active');

        $(this).parent().next().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    $(document).on('click', '.tab4 ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().parent().next().children().children('p.active').removeClass('active');

        $(this).parent().parent().next().children().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    // INSTAGRAM

    $(document).ready(function(){
        $(".instagram").owlCarousel(
            {
                items: 4,
                loop: true,
                autoplay: true,
                responsive:{
                    0:{
                        items:1
                    },
                    576:{
                        items:2
                    },
                    768:{
                        items:3
                    },
                    992:{
                        items:4
                    }
                }
            }
        );
      });

      $(document).ready(function(){
        $(".say").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });
})