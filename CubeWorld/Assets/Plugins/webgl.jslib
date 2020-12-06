mergeInto(LibraryManager.library, {
  GameToWebNative: function(tag, message) {
    if (typeof window.UnityGameToWebHandler === 'function') {
      window.UnityGameToWebHandler(Pointer_stringify(tag), Pointer_stringify(message));
    }
  }
});
