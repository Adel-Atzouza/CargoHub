import requests
import pytest

BASE_URL = "http://localhost:3000/api/v1/Shipments"  
headers = {'APIKEY': "Admin"}

def cleanup_test_data(test_name):
    response = requests.get(BASE_URL, headers=headers)
    if response.status_code == 200:
        shipments = response.json()
        for shipment in shipments:
            if "Test" in shipment["shipmentType"]:
                delete_response = requests.delete(f"{BASE_URL}/{shipment['id']}", headers=headers)
                assert delete_response.status_code in [200, 204], f"Cleanup failed in {test_name}"


def test_get_all_shipments():
    response = requests.get(BASE_URL, headers=headers)
    assert response.status_code == 200
    shipments = response.json()
    assert isinstance(shipments, list)
    assert len(shipments) > 0

def test_get_shipment_by_id():
    shipment_id = 1
    response = requests.get(f"{BASE_URL}/{shipment_id}", headers=headers)
    assert response.status_code == 200
    shipment = response.json()
    assert shipment["id"] == shipment_id

def test_add_shipment():
    "Test adding a new shipment."
    new_shipment = {
        "sourceId": 999,
        "orderDate": "2024-12-01T00:00:00Z",
        "requestDate": "2024-12-10T00:00:00Z",
        "shipmentDate": "2024-12-15T00:00:00Z",
        "shipmentType": "Test Shipment",
        "shipmentStatus": "TEST",
        "notes": "Test notes",
        "carrierCode": "TEST_CARRIER",
        "carrierDescription": "Test Carrier Description",
        "serviceCode": "TEST_SERVICE",
        "paymentType": "Prepaid",
        "transferMode": "Air",
        "totalPackageCount": 5,
        "totalPackageWeight": 50.0
    }
    response = requests.post(BASE_URL, json=new_shipment, headers=headers)
    assert response.status_code == 201, f"Failed to add shipment: {response.text}"

    # Print the response JSON for debugging
    print(response.json())

    # Verify the response contains the added shipment
    shipment = response.json()
    assert shipment["shipmentType"] == new_shipment["shipmentType"]

    cleanup_test_data("test_add_shipment")





