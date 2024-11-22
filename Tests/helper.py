def Contains_Keys(dictionary, keys):
    if (dictionary is None):
        return False
    return set(keys) == set(dictionary.keys())
