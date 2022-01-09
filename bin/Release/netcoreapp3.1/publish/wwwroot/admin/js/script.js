////(function () {
////	var PlaceHolderElement = $('#PlaceHolderHere');
////	$('button[data-toggle="ajax-modal"]').click(function (event) {
////		console.log("yerindedi");
////		var url = $(this).data('url');

////		$.get(url).done(function (data) {
////			PlaceHolderElement.html(data);
////			PlaceHolderElement.find('.modal').modal('show');

////		})

$("#complexConfirm").confirm({
    title: "Delete confirmation",
    text: "This is very dangerous, you shouldn't do it! Are you really really sure?",
    confirm: function (button) {
        alert("You just confirmed.");
    },
    cancel: function (button) {
        alert("You aborted the operation.");
    },
    confirmButton: "Yes I am",
    cancelButton: "No"
});
////	})