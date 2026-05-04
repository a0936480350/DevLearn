// Configure Prism autoloader to use local components instead of the cdnjs CDN.
// Without this, when a pre-loaded language script briefly fails to fetch (transient
// network or cache hash mismatch), Prism falls back to cdnjs, which the user's
// network may also block — producing duplicated frontend-resource errors.
(function () {
    if (window.Prism && Prism.plugins && Prism.plugins.autoloader) {
        Prism.plugins.autoloader.languages_path = '/lib/prism/components/';
    }
})();
