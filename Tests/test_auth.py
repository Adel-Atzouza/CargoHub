import pytest
import requests


Header_Full_Auth = {"Api_key": "a1b2c3d4e5"}
Header_only_get = {"Api_key": "f6g7h8i9j0"}
BASE_Url = "http://localhost:3000/api/v1"

endpoints = [

    # Clients
    {"url": "/clients", "method": "GET", "auth_key": Header_only_get},
    {"url": "/clients/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/clients", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/clients", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/clients/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/clients/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/clients/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/clients/1", "method": "DELETE", "auth_key": Header_Full_Auth},
    
    # Inventories
    {"url": "/inventories", "method": "GET", "auth_key": Header_only_get},
    {"url": "/inventories/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/inventories", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/inventories", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/inventories/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/inventories/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/inventories/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/inventories/1", "method": "DELETE", "auth_key": Header_Full_Auth},

    # item_groups
    {"url": "/item_groups", "method": "GET", "auth_key": Header_only_get},
    {"url": "/item_groups/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/item_groups", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/item_groups", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/item_groups/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/item_groups/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/item_groups/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/item_groups/1", "method": "DELETE", "auth_key": Header_Full_Auth},

    # item_lines
    {"url": "/item_lines", "method": "GET", "auth_key": Header_only_get},
    {"url": "/item_lines/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/item_lines", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/item_lines", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/item_lines/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/item_lines/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/item_lines/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/item_lines/1", "method": "DELETE", "auth_key": Header_Full_Auth},

    # item_types
    {"url": "/item_types", "method": "GET", "auth_key": Header_only_get},
    {"url": "/item_types/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/item_types", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/item_types", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/item_types/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/item_types/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/item_types/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/item_types/1", "method": "DELETE", "auth_key": Header_Full_Auth},

    # items
    {"url": "/items", "method": "GET", "auth_key": Header_only_get},
    {"url": "/items/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/items", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/items", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/items/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/items/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/items/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/items/1", "method": "DELETE", "auth_key": Header_Full_Auth},

    # locations
    {"url": "/locations", "method": "GET", "auth_key": Header_only_get},
    {"url": "/locations/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/locations", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/locations", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/locations/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/locations/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/locations/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/locations/1", "method": "DELETE", "auth_key": Header_Full_Auth},

    # orders
    {"url": "/orders", "method": "GET", "auth_key": Header_only_get},
    {"url": "/orders/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/orders", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/orders", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/orders/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/orders/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/orders/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/orders/1", "method": "DELETE", "auth_key": Header_Full_Auth},

    # shipments
    {"url": "/shipments", "method": "GET", "auth_key": Header_only_get},
    {"url": "/shipments/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/shipments", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/shipments", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/shipments/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/shipments/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/shipments/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/shipments/1", "method": "DELETE", "auth_key": Header_Full_Auth},

    # suppliers
    {"url": "/suppliers", "method": "GET", "auth_key": Header_only_get},
    {"url": "/suppliers/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/suppliers", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/suppliers", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/suppliers/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/suppliers/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/suppliers/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/suppliers/1", "method": "DELETE", "auth_key": Header_Full_Auth},

    # transfers
    {"url": "/transfers", "method": "GET", "auth_key": Header_only_get},
    {"url": "/transfers/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/transfers", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/transfers", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/transfers/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/transfers/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/transfers/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/transfers/1", "method": "DELETE", "auth_key": Header_Full_Auth},

    # warehouses
    {"url": "/warehouses", "method": "GET", "auth_key": Header_only_get},
    {"url": "/warehouses/1", "method": "GET", "auth_key": Header_only_get},
    {"url": "/warehouses", "method": "POST", "auth_key": Header_only_get},  # This should fail
    {"url": "/warehouses", "method": "POST", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/warehouses/1", "method": "PUT", "auth_key": Header_only_get},  # This should fail
    {"url": "/warehouses/1", "method": "PUT", "auth_key": Header_Full_Auth},  # This should succeed
    {"url": "/warehouses/1", "method": "DELETE", "auth_key": Header_only_get},  
    {"url": "/warehouses/1", "method": "DELETE", "auth_key": Header_Full_Auth},
]


@pytest.mark.parametrize("endpoint", endpoints)

# this test will run for all the urls in the endpoints list
def test_all_auth(endpoint):
    Url  = BASE_Url + endpoint["url"]
    Headers = endpoint["auth_key"]
    if endpoint["method"] == "GET":
        response = requests.get(Url, headers=Headers)
        assert response.status_code == 200  # GET requests should succeed for both keys


    elif endpoint["method"] == "POST":
        response = requests.post(Url, headers=Headers, json={"id" : 1})
        if Headers == Header_Full_Auth:
            assert response.status_code == 201
        else:
            assert response.status_code == 401 # the user should be Unauthorized using this api_key
    elif endpoint["method"] == "PUT":
        response = requests.put(Url, headers=Headers, json={"id" : 1})
        if Headers == Header_Full_Auth:
            assert response.status_code == 200
        else:
            assert response.status_code == 401 # the user should be Unauthorized using this api_key

    elif endpoint["method"] == "DELETE":
        response = requests.delete(Url, headers=Headers)
        if Headers == Header_Full_Auth:
            assert response.status_code == 200
        else:
            assert response.status_code == 401  # the user should be Unauthorized using this api_key