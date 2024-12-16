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
    "description": ""
}

item_type_1 = {
    "id": 1,
    "name": "Desktop",
    "description": ""
}

item_type_1_updated = {
    "id": 1,
    "name": "Desktop",
    "description": "changed"
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
    "supplier_part_number": ""
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
    "supplier_part_number": "E-86805-uTM"
}

template_warehouse = {
    "id": 0,
    "code": "",
    "name": "",
    "address": "",
    "zip": "",
    "city": "",
    "province": "",
    "country": "",
    "contact": {
        "name": "",
        "phone": "",
        "email": ""
    },
    "created_at": "",
    "updated_at": ""
}

warehouse_1 = {
    "id": 2000,
    "code": "WJKADEJE",
    "name": "Holzminden air and road hub",
    "address": "Sibilla-Hendriks-Gasse 5/8",
    "zip": "05955",
    "city": "Holzminden",
    "province": "Saarland",
    "country": "DE",
    "contact": {
        "name": "Mirco Warmer",
        "phone": "+49(0)0048791642",
        "email": "katjaklotz@example.net"
    }
}

warehouse_1_updated = {
    "id": 2000,
    "code": "Changed",
    "name": "New name",
    "address": "Sibilla-Hendriks-Gasse 5/8",
    "zip": "05955",
    "city": "Holzminden",
    "province": "Saarland",
    "country": "DE",
    "contact": {
        "name": "Mirco Warmer",
        "phone": "+49(0)0048791642",
        "email": "katjaklotz@example.net"
    }
}


template_transfer = {
    "id": 0,
    "reference": "",
    "transfer_from": 0,
    "transfer_to": 0,
    "transfer_status": "",
    "created_at": "",
    "updated_at": "",
    "items": [
        {
            "item_id": "",
            "amount": 0
        }
    ]
}

transfer_1 = {
    "id": 2000000,
    "reference": "TR00019",
    "transfer_from": 9227,
    "transfer_to": 9354,
    "items": [
        {
            "item_id": "P001598",
            "amount": 32
        }
    ]
}

transfer_1_updated = {
    "id": 2000000,
    "reference": "TR00019",
    "transfer_from": 1000,
    "transfer_to": 93054,
    "items": [
        {
            "item_id": "P001598",
            "amount": 32
        }
    ]
}


template_supplier = {
    "id": 0,
    "code": "",
    "name": "",
    "address": "",
    "address_extra": "",
    "city": "",
    "zip_code": "",
    "province": "",
    "country": "",
    "contact_name": "",
    "phonenumber": "",
    "reference": "",
    "created_at": "",
    "updated_at": "",
}

supplier_1 = {
    "id": 2000,
    "code": "SUP0497",
    "name": "Neal-Hoffman",
    "address": "7032 Mindy Meadow",
    "address_extra": "Apt. 937",
    "city": "Lake Alex",
    "zip_code": "62913",
    "province": "Washington",
    "country": "Taiwan",
    "contact_name": "Jeffrey Larsen",
    "phonenumber": "(786)666-7146",
    "reference": "N-SUP0497",
}

supplier_1_updated = {
    "id": 2000,
    "code": "de",
    "name": "Neal-ededdeHoffman",
    "address": "7032 Mindy Meadow",
    "address_extra": "Apt. 937",
    "city": "Lake Alex",
    "zip_code": "62913",
    "province": "Washington",
    "country": "Taiwan",
    "contact_name": "Jeffrey Larsen",
    "phonenumber": "(786)666-7146",
    "reference": "N-SUP0497",
}


template_client = {
    "id": 0,
    "name": "",
    "address": "",
    "city": "",
    "zip_code": "",
    "province": "",
    "country": "",
    "contact_name": "",
    "contact_phone": "",
    "contact_email": "",
    "created_at": "",
    "updated_at": ""
}

client_1 = {
    "id": 20000,
    "name": "Raymond Inc",
    "address": "1296 Daniel Road Apt. 349",
    "city": "Pierceview",
    "zip_code": "28301",
    "province": "Colorado",
    "country": "United States",
    "contact_name": "Bryan Clark",
    "contact_phone": "242.732.3483x2573",
    "contact_email": "robertcharles@example.net"
}

client_1_updated = {
    "id": 20000,
    "name": "Roedolf westermijer",
    "address": "cort van de lindestraat 69",
    "city": "Pierceview",
    "zip_code": "28301",
    "province": "Colorado",
    "country": "United States",
    "contact_name": "Bryan Clark",
    "contact_phone": "242.732.3483x2573",
    "contact_email": "robertcharles@example.net"
}


template_inventory = {
    "id": 10,
    "item_id": "",
    "description": "",
    "item_reference": "",
    "locations": [],
    "total_on_hand": 0,
    "total_expected": 0,
    "total_ordered": 0,
    "total_allocated": 0,
    "total_available": 0,
    "created_at": "",
    "updated_at": ""
}

inventory_1 = {
    "id": 20000,
    "item_id": "P000001",
    "description": "Face-to-face clear-thinking complexity",
    "item_reference": "sjQ23408K",
    "locations": [
        3211,
        24700,
        14123,
        19538,
        31071,
        24701,
        11606,
        11817
    ],
    "total_on_hand": 262,
    "total_expected": 0,
    "total_ordered": 80,
    "total_allocated": 41,
    "total_available": 141
}

inventory_1_updated = {
    "id": 20000,
    "item_id": "P000001",
    "description": "Xanax drug of installation table",
    "item_reference": "sjQ23408K",
    "locations": [
        3211,
        24700,
        14123,
        19538,
        31071,
        24701,
        11606,
        11817
    ],
    "total_on_hand": 262,
    "total_expected": 0,
    "total_ordered": 80,
    "total_allocated": 41,
    "total_available": 141
}
