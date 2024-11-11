def have_same_structure(dict1, dict2):
    # First, check if both dictionaries have the same set of keys
    if set(dict1.keys()) != set(dict2.keys()):
        return False

    # Iterate through the keys and check structure recursively for nested dictionaries
    for key in dict1:
        if isinstance(dict1[key], dict) and isinstance(dict2[key], dict):
            # Recursively check nested dictionaries
            if not have_same_structure(dict1[key], dict2[key]):
                return False
        elif isinstance(dict1[key], dict) or isinstance(dict2[key], dict):
            # One is a dictionary and the other is not
            return False

    return True


template_item_type = {
    "id": 0,
    "name": "",
    "description": "",
    "created_at": "",
    "updated_at": ""

}

item_type_1 = {
    "id": 3,
    "name": "Desktop",
    "description": ""
}

item_type_1_updated = {
    "id": 3,
    "name": "Desktop",
    "description": "changed",
}

template_item = {
    "uid": "",
    "code": "",
    "description": "",
    "short_description": "",
    "upc_code": "",
    "model_number": "",
    "commodity_code": "",
    "item_line": 0,
    "item_group": 0,
    "item_type": 0,
    "unit_purchase_quantity": 0,
    "unit_order_quantity": 0,
    "pack_order_quantity": 0,
    "supplier_id": 0,
    "supplier_code": "",
    "supplier_part_number": "",
    "created_at": "",
    "updated_at": ""
}

item_1 = {
    "uid": "P000001",
    "code": "sjQ23408K",
    "description": "Face-to-face clear-thinking complexity",
    "short_description": "must",
    "upc_code": "6523540947122",
    "model_number": "63-OFFTq0T",
    "commodity_code": "oTo304",
    "item_line": 11,
    "item_group": 73,
    "item_type": 14,
    "unit_purchase_quantity": 47,
    "unit_order_quantity": 13,
    "pack_order_quantity": 11,
    "supplier_id": 34,
    "supplier_code": "SUP423",
    "supplier_part_number": "E-86805-uTM"
}

item_2 = {
    "id": 0,
    "uid": "P000001",
    "code": "sjQ23408K",
    "description": "Face-to-face clear-thinking complexity",
    "short_description": "must",
    "upc_code": "6523540947122",
    "model_number": "63-OFFTq0T",
    "commodity_code": "oTo304",
    "item_line": 11,
    "item_group": 73,
    "item_type": 14,
    "unit_purchase_quantity": 47,
    "unit_order_quantity": 13,
    "pack_order_quantity": 11,
    "supplier_id": 34,
    "supplier_code": "SUP423",
    "supplier_part_number": "E-86805-uTM"
}

item_1_updated = {
    "uid": "P000001",
    "code": "sjQ23408K",
    "description": "Changed",
    "short_description": "must",
    "upc_code": "6523540947122",
    "model_number": "63-OFFTq0T",
    "commodity_code": "oTo304",
    "item_line": 11,
    "item_group": 73,
    "item_type": 14,
    "unit_purchase_quantity": 47,
    "unit_order_quantity": 13,
    "pack_order_quantity": 11,
    "supplier_id": 34,
    "supplier_code": "SUP423",
    "supplier_part_number": "E-86805-uTM",
}
