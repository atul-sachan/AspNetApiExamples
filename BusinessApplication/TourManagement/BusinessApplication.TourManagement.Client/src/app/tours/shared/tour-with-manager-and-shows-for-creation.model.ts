import { TourWithManagerForCreation } from "./tour-with-manager-for-creation.model";
import { ShowForCreation } from "./show-for-creation.model";

export class TourWithManagerAndShowsForCreation extends TourWithManagerForCreation {
    shows: ShowForCreation[];
}
